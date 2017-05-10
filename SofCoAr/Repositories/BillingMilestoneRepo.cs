
using SofCoAr.Models;
using SofCoAr.Repositories.Repositories.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using SofCoAr.Models.DTO;
using SofCoAr.Helper;

namespace SofCoAr.Repositories
{
    public class BillingMilestoneRepo : BaseRepo<BillingMilestone>, IBillingMilestoneRepo
    {
        public BillingMilestoneRepo(SofcoContext _context = null) : base(_context)
        {
        }

        public IEnumerable<BillingMilestone> GetAllFiltered(int idCustomer, int idService, int idProject)
        {

            var list = _context.BillingMilestones
                   .Include(x => x.SolFacState).AsNoTracking()
                   .Include(x => x.SolFacHists).AsNoTracking()
                   .Include(x => x.SolFacActionHists).AsNoTracking()
                   .Where(s => s.IdProject == idProject
                            && s.IdCustomer == idCustomer
                            && s.IdService == idService).ToList();

            foreach(var item in list)
            {
                foreach(var hist in item.SolFacHists)
                {
                    hist.SolFacState = _context.SolFacStates.Where(x => x.Id == hist.IdSolFacState).AsNoTracking().FirstOrDefault();
                    hist.User = _context.Users.Where(x => x.Id == hist.IdUser).AsNoTracking().FirstOrDefault();
                }
                foreach(var action in item.SolFacActionHists)
                {
                    action.User = _context.Users.Where(x => x.Id == action.IdUser).AsNoTracking().FirstOrDefault();
                    action.SolFacActionState = _context.SolFacActionStates.Where(x => x.Id == action.IdSolFacActionState).AsNoTracking().FirstOrDefault();
                    action.BillingMilestone = _context.BillingMilestones.Where(x => x.Id == action.IdMilestone).AsNoTracking().FirstOrDefault();
                    action.BillingMilestoneSource = _context.BillingMilestones.Where(x => x.Id == action.IdMilestoneSource).AsNoTracking().FirstOrDefault();
                }
            }
            
            return list;
        }

        public IEnumerable<BillingMilestone> GetAllFilteredWithDetails(int idCustomer, int idService, int idProject)
        {
            var app_context = (SofcoContext)_context;

             var dbQuery = app_context.BillingMilestones
                    .Include(x => x.BillingMilestoneDetails)
                    .Where(s => s.IdProject == idProject
                             && s.IdCustomer == idCustomer
                             && s.IdService == idService).ToList();


            var list = dbQuery;

            return list;
        }

        public override BillingMilestone GetById(int Id)
        {
            return _context.Set<BillingMilestone>()
                .Include("BillingMilestoneDetails")
                .Include("Project")
                .Where(m => m.Id == Id).FirstOrDefault();
        }

        public override void Add(BillingMilestone o)
        {
            o.Name = o.ProjectName + o.ContractNumber;
            base.Add(o);
        }

        public void AddHito(BillingMilestoneEditDTO o)
        {
            AddHito_private(o.BillingMilestone, o.IdUser, null);
        }

        public void EditHito(BillingMilestone o, int idUser, bool dividido = false)
        {

            var item = new BillingMilestone();
            ReflectionHelper.SetSimplePropertyValues<BillingMilestone>(o, ref item);

            ClearAddedEntries();

            _context.Entry(item).State = EntityState.Modified;

            //Grabar Historico de Acciones como Agregar, Editar, Dividir
            SolFacActionHistRepo repoActionHist = new SolFacActionHistRepo(base._context);
            //ultimo monto
            SolFacActionHist ultimoHist = SolFacActionsHelper.GetLastMilestoneAmount(_context, o.Id);
            decimal? ultimoMonto = null;
            if (ultimoHist != null)
            {
                ultimoMonto = ultimoHist.MontoNuevo;
            }
            //inicializo un registro del historial
            SolFacActionHist actionHist = new SolFacActionHist();
            actionHist.IdMilestone = o.Id;
            actionHist.Fecha = DateTime.Now;
            if (dividido)
            {
                actionHist.IdSolFacActionState = 3; //dividido
            }
            else
            {
                actionHist.IdSolFacActionState = 2; //modificado
            }

            actionHist.MontoAnte = ultimoMonto;
            actionHist.MontoNuevo = o.Monto;
            actionHist.IdUser = idUser;
            repoActionHist.Add(actionHist);
        }

        private void AddHito_private(BillingMilestone o, int idUser, int? idMilestoneSource = null)
        {

            //profile
            Profile profile = _context.Users
                .Where(u => u.Id == idUser)
                .Include(p => p.Profile)
                .Select(s => s.Profile)
                .FirstOrDefault();

            SolFacState currState = SolFacStateHelper.GetCurrentState(_context, o.Id);
            SolFacState prevState = SolFacStateHelper.GetPrevState(_context, currState);
            SolFacState nextState = SolFacStateHelper.GetNextState(_context, currState);

            //o.Name = o.ProjectName + o.ContractNumber;
            o.CurrState = nextState.Code;
            o.IdSolFacState = nextState.Id;

            if (o.MontoInic == null)
            {
                o.MontoInic = o.Monto;
            }

            ClearAddedEntries();

            base.Add(o);

            base._context.SaveChanges();



            //historial de aprobaciones o rechazos
            var repoHist = new SolFacHistRepo(_context);
            SolFacHist solfacthist = new SolFacHist();
            solfacthist.IdBillingMilestone = o.Id;
            solfacthist.IdSolFacState = nextState.Id;
            solfacthist.Canceled = false;
            solfacthist.IdUser = idUser;
            solfacthist.Date = DateTime.Now;
            repoHist.Add(solfacthist);


            //ultimo monto
            //si es nuevo nuevo, este dato vendrá en null.
            //Si vengo de Dividir, este dato tendrá algún monto
            SolFacActionHist ultimoHist = SolFacActionsHelper.GetLastMilestoneAmount(_context, idMilestoneSource);
            decimal? ultimoMonto = null;
            if (ultimoHist != null)
            {
                if(idMilestoneSource != null)
                {
                    ultimoMonto = ultimoHist.MontoAnte;
                }
                else
                {
                    ultimoMonto = ultimoHist.MontoNuevo;
                }
                
            }

            //Historial de acciones como Agregar, Modificar o Dividir
            SolFacActionHistRepo repoActionHist = new SolFacActionHistRepo(_context);
            SolFacActionHist actionHist = new SolFacActionHist();
            actionHist.IdMilestone = o.Id;
            actionHist.Fecha = DateTime.Now;
            if (idMilestoneSource == null)
            {
                actionHist.IdSolFacActionState = 1; //creado
            }
            else
            {
                actionHist.IdSolFacActionState = 3; //dividido
            }
            actionHist.MontoAnte = ultimoMonto;
            actionHist.MontoNuevo = o.Monto;
            actionHist.IdUser = idUser;
            actionHist.IdMilestoneSource = idMilestoneSource;
            repoActionHist.Add(actionHist);

            //guardar cambios
            _context.SaveChanges();


        }


        public void DivideHito(BillingMilestoneDivideDTO o)
        {

            //por cada Hito, crear un hito nuevo
            for (int i = 0; i < o.HitosDivididos.Length; i++)
            {
                if (i == 0)
                {
                    EditHito(o.HitosDivididos[i], o.IdUser, true);
                    _context.SaveChanges();
                }
                else
                {
                    o.HitosDivididos[i].Id = 0;

                    BillingMilestone hito = new BillingMilestone();
                    hito.Name = o.HitosDivididos[i].Name;
                    hito.Monto = o.HitosDivididos[i].Monto;
                    hito.MontoInic = hito.Monto;
                    hito.IdDocumentType = o.HitosDivididos[i].IdDocumentType;
                    hito.IdCustomer = o.HitosDivididos[i].IdCustomer;
                    hito.IdProject = o.HitosDivididos[i].IdProject;
                    hito.IdService = o.HitosDivididos[i].IdService;
                    hito.ScheduledDate = DateTime.Today;
                    hito.IdPaymentMethod = o.HitosDivididos[i].IdPaymentMethod;

                    AddHito_private(hito, o.IdUser, o.BillingMilestone.Id);

                    /* lstRepo[i-1] = new BillingMilestoneRepository();

                     hitosDivididos[i].Id = 0;
                     //hitosDivididos[i].Customer = null;
                     //hitosDivididos[i].DocumentType = null;
                     //hitosDivididos[i].BillingMilestoneDetails = null;
                     //hitosDivididos[i].PaymentMethod = null;
                     //hitosDivididos[i].SolFacState = null;
                     lstRepo[i-1].SaveOrUpdate_New(hitosDivididos[i], idUser, o.Id);*/
                }
            }

        }


        //public void SaveOrUpdate_Edit(BillingMilestone o, int idUser, bool dividido = false)
        //{
        //    SofcoContext app_context = (SofcoContext)_context;
        //    //BillingMilestoneDetailRepository repoDeta;

        //    //profile
        //    Profile profile = app_context.Users.Where(u => u.Id == idUser).Include(p => p.Profile).Select(s => s.Profile).FirstOrDefault();

        //    //historial
        //    //var repoHist = new SolFacHistRepository(base._context);

        //    //SolFacState currState = SolFacStateHelper.GetCurrentState(app_context, o.Id);
        //    //SolFacState prevState = SolFacStateHelper.GetPrevState(app_context, currState);
        //    //SolFacState nextState = SolFacStateHelper.GetNextState(app_context, currState);

        //    //Encabezado
        //    //o.CurrState = currState.Code;
        //    //o.IdSolFacState = currState.Id;
        //    //if (o.MontoInic == null)
        //    //{
        //    //    o.MontoInic = o.Monto;
        //    //}

        //    base.Edit(o);

        //    //base._context.SaveChanges();

        //    //Grabar Historico de Acciones como Agregar, Editar, Dividir
        //    SolFacActionHistRepo repoActionHist = new SolFacActionHistRepo(base._context);
        //    //ultimo monto
        //    SolFacActionHist ultimoHist = SolFacActionsHelper.GetLastMilestoneAmount(app_context, o.Id);
        //    decimal? ultimoMonto = null;
        //    if (ultimoHist != null)
        //    {
        //        ultimoMonto = ultimoHist.MontoNuevo;
        //    }
        //    //inicializo un registro del historial
        //    SolFacActionHist actionHist = new SolFacActionHist();
        //    actionHist.IdMilestone = o.Id;
        //    actionHist.Fecha = DateTime.Now;
        //    if (dividido)
        //    {
        //        actionHist.IdSolFacActionState = 3; //dividido
        //    }
        //    else
        //    {
        //        actionHist.IdSolFacActionState = 2; //modificado
        //    }

        //    actionHist.MontoAnte = ultimoMonto;
        //    actionHist.MontoNuevo = o.Monto;
        //    actionHist.IdUser = idUser;
        //    repoActionHist.Add(actionHist);

        //    //guardar cambios
        //    base._context.SaveChanges();

        //}


        public void AproveRejectHito(BillingMilestone o, int idUser, Boolean rechazar)
        {
            BillingMilestoneDetailRepo repoDeta;

            //var oCopy = new BillingMilestone();
            //ReflectionHelper.SetSimplePropertyValues<BillingMilestone>(o, ref oCopy);

            //profile
            Profile profile = _context.Users.Where(u => u.Id == idUser).Include(p => p.Profile).Select(s => s.Profile).FirstOrDefault();

            //historial
            var repoHist = new SolFacHistRepo(_context);

            SolFacState currState = SolFacStateHelper.GetCurrentState(_context, o.Id);
            SolFacState prevState = SolFacStateHelper.GetPrevState(_context, currState);
            SolFacState nextState = SolFacStateHelper.GetNextState(_context, currState);

            //solo hacer todo si existe el siguiente estado
            //si es siguiente estado es nulo, es porque la acción no está permitida
            if (rechazar == false && nextState != null || rechazar && prevState != null)
            {
                //Encabezado
                if (rechazar)
                {
                    o.CurrState = prevState.Code;
                    o.IdSolFacState = prevState.Id;
                }
                else
                {
                    o.CurrState = nextState.Code;
                    o.IdSolFacState = nextState.Id;
                }

                ClearAddedEntries();
                //ClearModifiedEntries();

                if (o.BillingMilestoneDetails.Count > 0)
                {
                    foreach (var d1 in o.BillingMilestoneDetails)
                    {
                        if (d1.Id > 0)
                        {
                            _context.Entry<BillingMilestoneDetail>(d1).State = EntityState.Modified;
                        }
                        else
                        {
                            _context.Entry<BillingMilestoneDetail>(d1).State = EntityState.Added;
                        }
                    }
                }

                _context.Entry<Project>(o.Project).State = EntityState.Unchanged;

                base.Edit(o);

                _context.SaveChanges();
                

                //historial de aprobaciones o rechazos
                SolFacHist solfacthist = new SolFacHist();
                solfacthist.IdBillingMilestone = o.Id;

                if (rechazar)
                {
                    solfacthist.IdSolFacState = prevState.Id;
                }
                else
                {
                    solfacthist.IdSolFacState = nextState.Id;
                }

                solfacthist.Canceled = rechazar;
                solfacthist.IdUser = idUser;
                solfacthist.Date = DateTime.Now;
                repoHist.Add(solfacthist);


                //guardar cambios
                _context.SaveChanges();

            }


        }

        //public void SaveOrUpdate_Divide(BillingMilestone o, int idUser, BillingMilestone[] hitosDivididos)
        //{
        //    SofcoContext app_context = (SofcoContext)_context;

        //    //por cada Hito, crear un hito nuevo
        //    for (int i = 0; i < hitosDivididos.Length; i++)
        //    {
        //        if (i == 0)
        //        {
        //            //SaveOrUpdate_Edit(hitosDivididos[i], idUser, true);
        //        }
        //        else
        //        {
        //            hitosDivididos[i].Id = 0;

        //            BillingMilestone hito = new BillingMilestone();
        //            hito.Name = hitosDivididos[i].Name;
        //            hito.Monto = hitosDivididos[i].Monto;
        //            hito.MontoInic = hito.Monto;
        //            hito.IdDocumentType = hitosDivididos[i].IdDocumentType;
        //            hito.IdCustomer = hitosDivididos[i].IdCustomer;
        //            hito.IdProject = hitosDivididos[i].IdProject;
        //            hito.IdService = hitosDivididos[i].IdService;

        //            AddHito_private(hitosDivididos[i], idUser, o.Id);

        //            /* lstRepo[i-1] = new BillingMilestoneRepository();

        //             hitosDivididos[i].Id = 0;
        //             //hitosDivididos[i].Customer = null;
        //             //hitosDivididos[i].DocumentType = null;
        //             //hitosDivididos[i].BillingMilestoneDetails = null;
        //             //hitosDivididos[i].PaymentMethod = null;
        //             //hitosDivididos[i].SolFacState = null;
        //             lstRepo[i-1].SaveOrUpdate_New(hitosDivididos[i], idUser, o.Id);*/
        //        }
        //    }

        //}

        /*
        public void SaveOrUpdate_Divide(BillingMilestone o, int idUser, BillingMilestone[] hitosDivididos)
        {
            SofcoContext app_context = (SofcoContext)_context;

            //BillingMilestoneRepository[] lstRepo = new BillingMilestoneRepository[hitosDivididos.Length-1];

            //por cada Hito, crear un hito nuevo
            for (int i = 0; i < hitosDivididos.Length; i++)
            {
                if (i == 0)
                {
                    SaveOrUpdate_Edit(hitosDivididos[i], idUser, true);
                }
                else
                {
                    hitosDivididos[i].Id = 0;

                    BillingMilestone hito = new BillingMilestone();
                    hito.Name = hitosDivididos[i].Name;
                    hito.Monto = hitosDivididos[i].Monto;
                    hito.MontoInic = hito.Monto;
                    hito.IdDocumentType = hitosDivididos[i].IdDocumentType;
                    hito.IdCustomer = hitosDivididos[i].IdCustomer;
                    hito.IdProject = hitosDivididos[i].IdProject;
                    hito.IdService = hitosDivididos[i].IdService;

                    SaveOrUpdate_New(hitosDivididos[i], idUser, o.Id);

                     //lstRepo[i-1] = new BillingMilestoneRepository();

                     //hitosDivididos[i].Id = 0;
                     ////hitosDivididos[i].Customer = null;
                     ////hitosDivididos[i].DocumentType = null;
                     ////hitosDivididos[i].BillingMilestoneDetails = null;
                     ////hitosDivididos[i].PaymentMethod = null;
                     ////hitosDivididos[i].SolFacState = null;
                     //lstRepo[i-1].SaveOrUpdate_New(hitosDivididos[i], idUser, o.Id);
                }
            }

        }
        */


            

        /*public void SaveOrUpdate2(BillingMilestone o, int idUser, Boolean simple, Boolean update, Boolean rechazar, Boolean dividir, BillingMilestone[] hitosDivididos)
        {
            SofcoContext app_context = (SofcoContext) _context;
            BillingMilestoneDetailRepository repoDeta;
            SolFacActionHistRepository repoActionHist;

            //profile
            Profile profile = app_context.Users.Where(u => u.Id == idUser).Include(p => p.Profile).Select(s => s.Profile).FirstOrDefault();

            //historial
            var repoHist = new SolFacHistRepository(base._context);

            SolFacState currState = SolFacStateHelper.GetCurrentState(app_context, o.Id);
            SolFacState prevState = SolFacStateHelper.GetPrevState(app_context, currState);
            SolFacState nextState = SolFacStateHelper.GetNextState(app_context, currState);

            //solo hacer todo si existe el siguiente estado
            //si es siguiente estado es nulo, es porque la acción no está permitida
            if (rechazar == false && nextState != null || rechazar && prevState != null)
            {
                //Encabezado
                //o.Name = o.ProjectName + o.ContractNumber;
                if (update)
                {
                    o.CurrState = currState.Code;
                    o.IdSolFacState = currState.Id;
                    if (o.MontoInic == null)
                    {
                        o.MontoInic = o.Monto;
                    }
                    
                }
                else
                {
                    if (rechazar)
                    {
                        o.CurrState = prevState.Code;
                        o.IdSolFacState = prevState.Id;
                    }
                    else
                    {
                        o.CurrState = nextState.Code;
                        o.IdSolFacState = nextState.Id;
                    }
                }
                base.SaveOrUpdate(o);

                base._context.SaveChanges();

                if (!update)
                {
                    //historial
                    SolFacHist solfacthist = new SolFacHist();
                    solfacthist.IdBillingMilestone = o.Id;

                    if (rechazar)
                    {
                        solfacthist.IdSolFacState = nextState.Id; ;
                    }
                    else
                    {
                        solfacthist.IdSolFacState = nextState.Id;
                    }
                    
                    solfacthist.Canceled = rechazar;
                    solfacthist.IdUser = idUser;
                    solfacthist.Date = DateTime.Now;
                    repoHist.SaveOrUpdate(solfacthist);

                }

                //simple es cuando se agrega un hito nuevo o cuando se edita
                if(!simple)
                {
                    //detalles
                    repoDeta = new BillingMilestoneDetailRepository(base._context);
                    if (o.BillingMilestoneDetails != null)
                    {
                        foreach (var d in o.BillingMilestoneDetails)
                        {
                            repoDeta.SaveOrUpdate(d);
                        }
                    }

                }
                else
                {
                    //Simple.
                    //Grabar SolFacActionHists
                    repoActionHist = new SolFacActionHistRepository(base._context);

                    //ultimo monto
                    SolFacActionHist ultimoHist = SolFacActionsHelper.GetLastMilestoneAmount(app_context, o.Id);
                    decimal? ultimoMonto = null;
                    if (ultimoHist != null)
                    {
                        ultimoMonto = ultimoHist.MontoNuevo;
                    }

                    //inicializo un registro del historial
                    SolFacActionHist actionHist = new SolFacActionHist();
                    actionHist.IdMilestone = o.Id;
                    actionHist.Fecha = DateTime.Now;
                    if(update && dividir)
                    {
                        actionHist.IdMilestoneSource = o.Id;
                        actionHist.IdSolFacActionState = 3; //dividido


                    } else if(update && !dividir)
                    {
                        actionHist.IdSolFacActionState = 2; //modificado
                        actionHist.MontoAnte = ultimoMonto;
                        actionHist.MontoNuevo = o.Monto;
                    } else
                    {
                        actionHist.IdSolFacActionState = 1; //creado
                        actionHist.MontoAnte = null;
                        actionHist.MontoNuevo = o.Monto;
                    }
                    actionHist.Iduser = idUser;

                }

                //guardar cambios
                base._context.SaveChanges();

                if (!simple)
                {
                    //limpiar recursos
                    repoDeta = null;
                }
                
            }
            

            
            
        }*/
    }
}
