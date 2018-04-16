/// <summary>
/// Finance Master Data API : This API is used to communicate with the Finance Master Entities
/// </summary>
/// 
namespace SmartBiz.MDMAPI.API
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Transactions;

    using SmartBiz.MDMAPI.API.Queries;
    using SmartBiz.MDMAPI.Common.Entities;
    using SmartBiz.MDMAPI.Data;
    using SmartBiz.PCSAPI.Common;
    using SmartBiz.PCSAPI.Common.Enums;
    using SmartBiz.Util;


    public class FM100 : IDisposable
    {

        public void Dispose()
        {
            GC.Collect();
        }

        #region AgentBrokerSalesMan
        /// <summary>
        /// Creates the agent broker sales man.
        /// </summary>
        /// <param name="new_ERP_AgentBrokerSalesMan">The new_ er p_ agent broker sales man.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateAgentBrokerSalesMan(ERP_AgentBrokerSalesMan new_ERP_AgentBrokerSalesMan)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        var generalLedgerAccount = ctx.FIN_GeneralLedgerAccount.Find(new_ERP_AgentBrokerSalesMan.FIN_GeneralLedgerAccount.AccountNo);
                        new_ERP_AgentBrokerSalesMan.FIN_GeneralLedgerAccount = generalLedgerAccount;
                        var currency = ctx.FIN_Currency.Find(new_ERP_AgentBrokerSalesMan.FIN_Currency.CurrencyCode);
                        new_ERP_AgentBrokerSalesMan.FIN_Currency = currency;
                        ctx.ERP_AgentBrokerSalesMan.Add(new_ERP_AgentBrokerSalesMan);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new AgentBrokerSalesMan : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Reads the agent broker sales manby key.
        /// </summary>
        /// <param name="AgentBrokerSalesManCode">The agent broker sales man code.</param>
        /// <param name="AgentBrokerSalesManFlag">The agent broker sales man flag.</param>
        /// <returns>ERP_AgentBrokerSalesMan.</returns>
        public ERP_AgentBrokerSalesMan ReadAgentBrokerSalesManbyKey(string AgentBrokerSalesManCode, int AgentBrokerSalesManFlag)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.ERP_AgentBrokerSalesMan.Where(c => c.AgentBrokerSalesManCode == AgentBrokerSalesManCode && c.AgentBrokerSalesManFlag == AgentBrokerSalesManFlag).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAgentBrokerSalesManbyKey : {0} ", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Return all AgentBrokerSalesMan records
        /// </summary>
        /// <returns>ERP_AgentBrokerSalesMan[].</returns>
        public ERP_AgentBrokerSalesMan[] ReadAllAgentBrokerSalesMan()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.ERP_AgentBrokerSalesMan.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAllAgentBrokerSalesMan : {0} ", ex.Message));
                return new ERP_AgentBrokerSalesMan[0];
            }
        }

        /// <summary>
        /// Execute the query and Return AgentBrokerSalesMan records
        /// </summary>
        /// <returns>IQueryable&lt;ERP_AgentBrokerSalesMan&gt;.</returns>
        public IQueryable<ERP_AgentBrokerSalesMan> ReadAgentBrokerSalesMan()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.ERP_AgentBrokerSalesMan.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAgentBrokerSalesMan : {0} ", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Update AgentBrokerSalesMan record
        /// </summary>
        /// <param name="modifiedAgentBrokerSalesMan">The modified agent broker sales man.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateAgentBrokerSalesMan(ERP_AgentBrokerSalesMan modifiedAgentBrokerSalesMan)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        ERP_AgentBrokerSalesMan original = ctx.ERP_AgentBrokerSalesMan.Find(modifiedAgentBrokerSalesMan.AgentBrokerSalesManCode);

                        if (original != null)
                        {
                            ctx.Entry(original).CurrentValues.SetValues(modifiedAgentBrokerSalesMan);
                            ctx.SaveChanges();

                            var generalLedgerAccount = ctx.FIN_GeneralLedgerAccount.Find(modifiedAgentBrokerSalesMan.FIN_GeneralLedgerAccount.AccountNo);
                            modifiedAgentBrokerSalesMan.FIN_GeneralLedgerAccount = generalLedgerAccount;
                            var currency = ctx.FIN_Currency.Find(modifiedAgentBrokerSalesMan.FIN_Currency.CurrencyCode);
                            modifiedAgentBrokerSalesMan.FIN_Currency = currency;
                            ctx.Entry(original).CurrentValues.SetValues(modifiedAgentBrokerSalesMan);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("Unit with AgentBrokerSalesManCode: {0}  was not found.", modifiedAgentBrokerSalesMan.AgentBrokerSalesManCode)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateAgentBrokerSalesMan : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Delete AgentBrokerSalesMan record
        /// </summary>
        /// <param name="deletingAgentBrokerSalesMan">The deleting agent broker sales man.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteAgentBrokerSalesMan(ERP_AgentBrokerSalesMan deletingAgentBrokerSalesMan)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        ERP_AgentBrokerSalesMan original = ctx.ERP_AgentBrokerSalesMan.Find(deletingAgentBrokerSalesMan.AgentBrokerSalesManCode);
                        if (original != null)
                        {
                            ctx.Entry(original).State = EntityState.Deleted;
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("Unit with AgentBrokerSalesManCode: {0}  was not found.", deletingAgentBrokerSalesMan.AgentBrokerSalesManCode)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteAgentBrokerSalesMan) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Queries the agent broker sales man.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;ERP_AgentBrokerSalesMan&gt;.</returns>
        public ResultDTO<ERP_AgentBrokerSalesMan> QueryAgentBrokerSalesMan(AgentBrokerSalesManQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.ERP_AgentBrokerSalesMan.AsQueryable();

                    if (query.AgentBrokerSalesManCode != null)
                    {
                        c = c.Where(i => i.AgentBrokerSalesManCode == query.AgentBrokerSalesManCode);
                    }
                    if (query.City != null)
                    {
                        c = c.Where(i => i.City == query.City);
                    }
                    if (query.Country != null)
                    {
                        c = c.Where(i => i.Country == query.Country);
                    }
                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.AgentBrokerSalesManCode).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<ERP_AgentBrokerSalesMan>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on QueryUnitConversion : {0} ", ex.Message));
                return null;
            }
        }
        /// <summary>
        /// Agents the broker sales man exists.
        /// </summary>
        /// <param name="AgentBrokerSalesManexists">The agent broker sales manexists.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool AgentBrokerSalesManExists(ERP_AgentBrokerSalesMan AgentBrokerSalesManexists)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    ERP_AgentBrokerSalesMan record = ctx.ERP_AgentBrokerSalesMan.Find(AgentBrokerSalesManexists.AgentBrokerSalesManCode, AgentBrokerSalesManexists.AgentBrokerSalesManFlag);
                    if (record != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on AgentBrokerSalesManExists : {0} ", ex.Message));
                return false;
            }
        }
        #endregion

        #region Region
        /// <summary>
        /// Creates the region.
        /// </summary>
        /// <param name="new_FIN_Region">The new_ fi n_ region.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateRegion(FIN_Region new_FIN_Region)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        ctx.FIN_Region.Add(new_FIN_Region);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new Region : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Reads the regionby key.
        /// </summary>
        /// <param name="RegionCode">The region code.</param>
        /// <returns>FIN_Region.</returns>
        public FIN_Region ReadRegionbyKey(string RegionCode)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_Region.Where(c => c.RegionCode == RegionCode).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadRegionbyKey : {0} ", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Return all Region records
        /// </summary>
        /// <returns>FIN_Region[].</returns>
        public FIN_Region[] ReadAllRegion()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_Region.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAllRegion : {0} ", ex.Message));
                return new FIN_Region[0];
            }
        }

        /// <summary>
        /// Execute the query and Return Region records
        /// </summary>
        /// <returns>IQueryable&lt;FIN_Region&gt;.</returns>
        public IQueryable<FIN_Region> ReadRegion()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_Region.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadRegion : {0} ", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Update Region record
        /// </summary>
        /// <param name="modifiedRegion">The modified region.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateRegion(FIN_Region modifiedRegion)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        FIN_Region original = ctx.FIN_Region.Find(modifiedRegion.RegionCode);

                        if (original != null)
                        {
                            ctx.Entry(original).CurrentValues.SetValues(modifiedRegion);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("Region with RegionCode: {0}  was not found.", modifiedRegion.RegionCode)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateRegion : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Delete Region record
        /// </summary>
        /// <param name="deletingRegion">The deleting region.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteRegion(FIN_Region deletingRegion)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        FIN_Region original = ctx.FIN_Region.Find(deletingRegion.RegionCode);
                        if (original != null)
                        {
                            ctx.Entry(original).State = EntityState.Deleted;
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("Region with RegionCode: {0}  was not found.", deletingRegion.RegionCode)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteRegion) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Regions the exists.
        /// </summary>
        /// <param name="Regionexists">The regionexists.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool RegionExists(FIN_Region Regionexists)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    FIN_Region record = ctx.FIN_Region.Find(Regionexists.RegionCode);
                    if (record != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on RegionExists : {0} ", ex.Message));
                return false;
            }
        }
        /// <summary>
        /// Queries the region.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;FIN_Region&gt;.</returns>
        public ResultDTO<FIN_Region> QueryRegion(RegionQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.FIN_Region.AsQueryable();

                    if (query.RegionCode != null)
                    {
                        c = c.Where(i => i.RegionCode == query.RegionCode);
                    }
                    if (query.RegionName != null)
                    {
                        c = c.Where(i => i.RegionName == query.RegionName);
                    }
                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.RegionCode).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<FIN_Region>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on QueryRegion : {0} ", ex.Message));
                return null;
            }
        }
        #endregion

        #region Area
        /// <summary>
        /// Creates the area.
        /// </summary>
        /// <param name="new_FIN_Area">The new_ fi n_ area.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateArea(FIN_Area new_FIN_Area)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        ctx.FIN_Area.Add(new_FIN_Area);
                        ctx.SaveChanges();

                        var region = ctx.FIN_Region.Find(new_FIN_Area.FIN_Region.RegionCode);
                        new_FIN_Area.FIN_Region = region;
                        ctx.FIN_Area.Add(new_FIN_Area);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new Area : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Reads the areaby key.
        /// </summary>
        /// <param name="AreaCode">The area code.</param>
        /// <param name="RegionCode">The region code.</param>
        /// <returns>FIN_Area.</returns>
        public FIN_Area ReadAreabyKey(string AreaCode, string RegionCode)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_Area.Where(c => c.AreaCode == AreaCode && c.RegionCode == RegionCode).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAreabyKey : {0} ", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Return all Area records
        /// </summary>
        /// <returns>FIN_Area[].</returns>
        public FIN_Area[] ReadAllArea()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_Area.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAllArea : {0} ", ex.Message));
                return new FIN_Area[0];
            }
        }

        /// <summary>
        /// Execute the query and Return Area records
        /// </summary>
        /// <returns>IQueryable&lt;FIN_Area&gt;.</returns>
        public IQueryable<FIN_Area> ReadArea()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_Area.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadArea : {0} ", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Update Area record
        /// </summary>
        /// <param name="modifiedArea">The modified area.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateArea(FIN_Area modifiedArea)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        FIN_Area original = ctx.FIN_Area.Find(modifiedArea.AreaCode, modifiedArea.RegionCode);

                        if (original != null)
                        {
                            var region = ctx.FIN_Region.Find(modifiedArea.FIN_Region.RegionCode);
                            modifiedArea.FIN_Region = region;
                            ctx.Entry(original).CurrentValues.SetValues(modifiedArea);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("Area with AreaCode: {0} RegionCode: {1}  was not found.", modifiedArea.AreaCode, modifiedArea.RegionCode)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateArea : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Delete Area record
        /// </summary>
        /// <param name="deletingArea">The deleting area.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteArea(FIN_Area deletingArea)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        FIN_Area original = ctx.FIN_Area.Find(deletingArea.AreaCode, deletingArea.RegionCode);
                        if (original != null)
                        {
                            ctx.Entry(original).State = EntityState.Deleted;
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("Area with AreaCode: {0} RegionCode: {1}  was not found.", deletingArea.AreaCode, deletingArea.RegionCode)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteArea) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Areas the exists.
        /// </summary>
        /// <param name="Areaexists">The areaexists.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool AreaExists(FIN_Area Areaexists)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    FIN_Region record = ctx.FIN_Region.Find(Areaexists.AreaCode, Areaexists.RegionCode);
                    if (record != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on AreaExists : {0} ", ex.Message));
                return false;
            }
        }
        /// <summary>
        /// Queries the area.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;FIN_Area&gt;.</returns>
        public ResultDTO<FIN_Area> QueryArea(AreaQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.FIN_Area.AsQueryable();

                    if (query.AreaCode != null)
                    {
                        c = c.Where(i => i.AreaCode == query.AreaCode);
                    }
                    if (query.RegionCode != null)
                    {
                        c = c.Where(i => i.RegionCode == query.RegionCode);
                    }
                    if (query.AreaName != null)
                    {
                        c = c.Where(i => i.AreaName == query.AreaName);
                    }
                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.RegionCode).Skip((pageNumber - 1) * pageSize).Take(pageSize).Include(s => s.FIN_Region).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<FIN_Area>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on QueryArea : {0} ", ex.Message));
                return null;
            }
        }

        #endregion

        #region UnitDefinition
        /// <summary>
        /// Creates the unit definition.
        /// </summary>
        /// <param name="new_ERP_UnitDefinition">The new_ er p_ unit definition.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateUnitDefinition(ERP_UnitDefinition new_ERP_UnitDefinition)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        var unitDefiniton = ctx.ERP_UnitDefinition.Find(new_ERP_UnitDefinition.ERP_UnitDefinition2.UnitCode);
                        new_ERP_UnitDefinition.ERP_UnitDefinition2 = unitDefiniton;
                        ctx.ERP_UnitDefinition.Add(new_ERP_UnitDefinition);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new UnitDefinition : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Reads the unit definitionby key.
        /// </summary>
        /// <param name="UnitDefinitionCode">The unit definition code.</param>
        /// <returns>ERP_UnitDefinition.</returns>
        public ERP_UnitDefinition ReadUnitDefinitionbyKey(string UnitDefinitionCode)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.ERP_UnitDefinition.Where(c => c.UnitCode == UnitDefinitionCode).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadUnitDefinitionbyKey : {0} ", ex.Message));
                return null;
            }
        }
        /// <summary>
        /// Return all UnitDefinition records
        /// </summary>
        /// <returns>ERP_UnitDefinition[].</returns>
        public ERP_UnitDefinition[] ReadAllUnitDefinition()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.ERP_UnitDefinition.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAllUnitDefinition : {0} ", ex.Message));
                return new ERP_UnitDefinition[0];
            }
        }
        /// <summary>
        /// Execute the query and Return UnitDefinition records
        /// </summary>
        /// <returns>IQueryable&lt;ERP_UnitDefinition&gt;.</returns>
        public IQueryable<ERP_UnitDefinition> ReadUnitDefinition()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.ERP_UnitDefinition.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadUnitDefinition : {0} ", ex.Message));
                return null;
            }
        }
        /// <summary>
        /// Update UnitDefinition record
        /// </summary>
        /// <param name="modifiedUnitDefinition">The modified unit definition.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateUnitDefinition(ERP_UnitDefinition modifiedUnitDefinition)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        ERP_UnitDefinition original = ctx.ERP_UnitDefinition.Find(modifiedUnitDefinition.UnitCode);

                        if (original != null)
                        {
                            var unitDefiniton = ctx.ERP_UnitDefinition.Find(modifiedUnitDefinition.ERP_UnitDefinition2.UnitCode);
                            modifiedUnitDefinition.ERP_UnitDefinition2 = unitDefiniton;
                            ctx.Entry(original).CurrentValues.SetValues(modifiedUnitDefinition);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("UnitDefinition with UnitCode: {0}  was not found.", modifiedUnitDefinition.UnitCode)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateUnitDefinition : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Delete UnitDefinition record
        /// </summary>
        /// <param name="deletingUnitDefinition">The deleting unit definition.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteUnitDefinition(ERP_UnitDefinition deletingUnitDefinition)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        ERP_UnitDefinition original = ctx.ERP_UnitDefinition.Find(deletingUnitDefinition.UnitCode);
                        if (original != null)
                        {
                            ctx.Entry(original).State = EntityState.Deleted;
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("UnitDefinition with UnitCode: {0}  was not found.", deletingUnitDefinition.UnitCode)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteUnitDefinition) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Queries the unit definition.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;ERP_UnitDefinition&gt;.</returns>
        public ResultDTO<ERP_UnitDefinition> QueryUnitDefinition(UnitDefinitionQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    var c = ctx.ERP_UnitDefinition.AsQueryable();

                    if (query.UnitCode != null)
                    {
                        c = c.Where(i => i.UnitCode == query.UnitCode);
                    }
                    if (query.MajorUnitCode != null)
                    {
                        c = c.Where(i => i.MajorUnitCode == query.MajorUnitCode);
                    }
                    if (query.StandardSyntax != null)
                    {
                        c = c.Where(i => i.StandardSyntax == query.StandardSyntax);
                    }
                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.UnitCode).Skip((pageNumber - 1) * pageSize).Take(pageSize).Include(s => s.ERP_UnitDefinition2).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<ERP_UnitDefinition>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on QueryUnitDefinition : {0} ", ex.Message));
                return null;
            }
        }
        /// <summary>
        /// Units the definition exists.
        /// </summary>
        /// <param name="UnitDefinitionexists">The unit definitionexists.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool UnitDefinitionExists(ERP_UnitDefinition UnitDefinitionexists)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    ERP_UnitDefinition record = ctx.ERP_UnitDefinition.Find(UnitDefinitionexists.UnitCode);
                    if (record != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UnitDefinitionExists : {0} ", ex.Message));
                return false;
            }
        }
        #endregion

        #region UnitConversion
        /// <summary>
        /// Creates the unit conversion.
        /// </summary>
        /// <param name="new_ERP_UnitConversion">The new_ er p_ unit conversion.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateUnitConversion(ERP_UnitConversion new_ERP_UnitConversion)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        var unitDefiniton = ctx.ERP_UnitDefinition.Find(new_ERP_UnitConversion.ERP_UnitDefinition.UnitCode);
                        new_ERP_UnitConversion.ERP_UnitDefinition = unitDefiniton;
                        ctx.ERP_UnitConversion.Add(new_ERP_UnitConversion);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new UnitConversion : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Reads the unit conversionby key.
        /// </summary>
        /// <param name="FromUnitCode">From unit code.</param>
        /// <param name="ToUnitCode">To unit code.</param>
        /// <returns>ERP_UnitConversion.</returns>
        public ERP_UnitConversion ReadUnitConversionbyKey(string FromUnitCode, string ToUnitCode)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.ERP_UnitConversion.Where(c => c.FromUnitCode == FromUnitCode && c.ToUnitCode == ToUnitCode).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadUnitConversionbyKey : {0} ", ex.Message));
                return null;
            }
        }
        /// <summary>
        /// Return all UnitConversion records
        /// </summary>
        /// <returns>ERP_UnitConversion[].</returns>
        public ERP_UnitConversion[] ReadAllUnitConversion()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.ERP_UnitConversion.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAllUnitConversion : {0} ", ex.Message));
                return new ERP_UnitConversion[0];
            }
        }
        /// <summary>
        /// Execute the query and Return UnitConversion records
        /// </summary>
        /// <returns>IQueryable&lt;ERP_UnitConversion&gt;.</returns>
        public IQueryable<ERP_UnitConversion> ReadUnitConversion()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.ERP_UnitConversion.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadUnitConversion : {0} ", ex.Message));
                return null;
            }
        }
        /// <summary>
        /// Update UnitConversion record
        /// </summary>
        /// <param name="modifiedUnitCoversion">The modified unit coversion.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateUnitConversion(ERP_UnitConversion modifiedUnitCoversion)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        ERP_UnitConversion original = ctx.ERP_UnitConversion.Find(modifiedUnitCoversion.FromUnitCode, modifiedUnitCoversion.ToUnitCode);

                        if (original != null)
                        {
                            var unitDefiniton = ctx.ERP_UnitDefinition.Find(modifiedUnitCoversion.ERP_UnitDefinition.UnitCode);
                            modifiedUnitCoversion.ERP_UnitDefinition = unitDefiniton;
                            ctx.Entry(original).CurrentValues.SetValues(modifiedUnitCoversion);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("UnitConversion with FromUnitCode: {0}  was not found.", modifiedUnitCoversion.FromUnitCode)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateUnitConversion : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Delete UnitConversion record
        /// </summary>
        /// <param name="deletingUnitConversion">The deleting unit conversion.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteUnitConversion(ERP_UnitConversion deletingUnitConversion)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        ERP_UnitConversion original = ctx.ERP_UnitConversion.Find(deletingUnitConversion.FromUnitCode);
                        if (original != null)
                        {
                            ctx.Entry(original).State = EntityState.Deleted;
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("UnitConversion with FromUnitCode: {0}  was not found.", deletingUnitConversion.FromUnitCode)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteUnitConversion) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Queries the unit conversion.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;ERP_UnitConversion&gt;.</returns>
        public ResultDTO<ERP_UnitConversion> QueryUnitConversion(UnitConversionQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    var c = ctx.ERP_UnitConversion.AsQueryable();

                    if (query.FromUnitCode != null)
                    {
                        c = c.Where(i => i.FromUnitCode == query.FromUnitCode);
                    }
                    if (query.ToUnitCode != null)
                    {
                        c = c.Where(i => i.ToUnitCode == query.ToUnitCode);
                    }
                    if (query.ConversionFactor != null)
                    {
                        c = c.Where(i => i.ConversionFactor == query.ConversionFactor);
                    }
                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.FromUnitCode).Skip((pageNumber - 1) * pageSize).Take(pageSize).Include(s => s.ERP_UnitDefinition).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<ERP_UnitConversion>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on QueryUnitConversion : {0} ", ex.Message));
                return null;
            }
        }
        /// <summary>
        /// Units the conversion exists.
        /// </summary>
        /// <param name="UnitConversionexists">The unit conversionexists.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool UnitConversionExists(ERP_UnitConversion UnitConversionexists)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    ERP_UnitConversion record = ctx.ERP_UnitConversion.Find(UnitConversionexists.FromUnitCode, UnitConversionexists.ToUnitCode);
                    if (record != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UnitConversionExists : {0} ", ex.Message));
                return false;
            }
        }
        #endregion

        #region Document

        /// <summary>
        /// Creates the document.
        /// </summary>
        /// <param name="new_ERP_Document">The new_ er p_ document.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateDocument(ERP_Document new_ERP_Document)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        ctx.ERP_Document.Add(new_ERP_Document);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };

            }

            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new Document : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        public ResultDTO<ERP_Document> QueryDocument(DocumentQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.ERP_Document.AsQueryable();

                    if (query.DocCode != null)
                    {
                        c = c.Where(i => i.DocCode == query.DocCode);
                    }
                    if (query.DocName != null)
                    {
                        c = c.Where(i => i.DocName == query.DocName);
                    }
                    if (query.SubSystemCode != null)
                    {
                        c = c.Where(i => i.SubSystemCode == query.SubSystemCode);
                    }
                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.DocCode).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<ERP_Document>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on QueryDocument : {0} ", ex.Message));
                return null;
            }
        }
        public bool DocumentExists(ERP_Document Documentexists)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    ERP_Document record = ctx.ERP_Document.Find(Documentexists.DocCode);
                    if (record != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DocumentExists : {0} ", ex.Message));
                return false;
            }
        }
        public ERP_Document ReadDocumentbyKey(string DocCode)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.ERP_Document.Where(c => c.DocCode == DocCode).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadDocumentbyKey : {0} ", ex.Message));
                return null;
            }
        }


        /// <summary>
        ///     Return all PrimaryTransaction records
        /// </summary>
        /// <returns></returns>
        public ERP_Document[] ReadAllDocument()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.ERP_Document.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAllDocument : {0} ", ex.Message));
                return new ERP_Document[0];
            }
        }

        /// <summary>
        ///     Execute the query and Return PrimaryTransaction records
        /// </summary>
        /// <returns></returns>
        public IQueryable<ERP_Document> ReadDocument()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.ERP_Document.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadDocument : {0} ", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Update Document record
        /// </summary>
        /// <param name="modified"></param>
        /// <returns></returns>
        public ApiAck UpdateDocument(ERP_Document modifiedDocument)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        ERP_Document original = ctx.ERP_Document.Find(modifiedDocument.DocCode);

                        if (original != null)
                        {
                            ctx.Entry(original).CurrentValues.SetValues(modifiedDocument);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("Document with DocCode:{0} was not found.", modifiedDocument.DocCode)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateDocument : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Delete Document record
        /// </summary>
        /// <param name="deleting"></param>
        /// <returns></returns>
        public ApiAck DeleteDocument(ERP_Document deletingDocument)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        ERP_Document original = ctx.ERP_Document.Find(deletingDocument.DocCode);

                        if (original != null)
                        {
                            ctx.Entry(original).State = EntityState.Deleted;
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("PrimaryTransaction with DocCode:{0} was not found.", deletingDocument.DocCode)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteDocument) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        #endregion

        #region DocumentAttributes

        /// <summary>
        /// Creates the document attributes.
        /// </summary>
        /// <param name="new_ERP_DocumentAttributes">The new_ er p_ document attributes.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateDocumentAttributes(ERP_DocumentAttributes new_ERP_DocumentAttributes)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        var document = ctx.ERP_Document.Find(new_ERP_DocumentAttributes.ERP_Document.DocCode);
                        new_ERP_DocumentAttributes.ERP_Document = document;
                        ctx.ERP_DocumentAttributes.Add(new_ERP_DocumentAttributes);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };

            }

            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new DocumentAttributes : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }


        /// <summary>
        /// Queries the document attributes.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;ERP_DocumentAttributes&gt;.</returns>
        public ResultDTO<ERP_DocumentAttributes> QueryDocumentAttributes(DocumentAttributesQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.ERP_DocumentAttributes.AsQueryable();

                    if (query.DocCode != null)
                    {
                        c = c.Where(i => i.DocCode == query.DocCode);
                    }
                    if (query.TxCode != null)
                    {
                        c = c.Where(i => i.TxCode == query.TxCode);
                    }
                    if (query.ShortName != null)
                    {
                        c = c.Where(i => i.ShortName.Contains(query.ShortName));
                    }
                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.DocCode).Skip((pageNumber - 1) * pageSize).Take(pageSize).Include(s => s.ERP_Document).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<ERP_DocumentAttributes>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on QueryDocumentAttributes : {0} ", ex.Message));
                return null;
            }
        }
        /// <summary>
        /// Documents the attributes exists.
        /// </summary>
        /// <param name="DocumentAttributesexists">The document attributesexists.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool DocumentAttributesExists(ERP_DocumentAttributes DocumentAttributesexists)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    ERP_DocumentAttributes record = ctx.ERP_DocumentAttributes.Find(DocumentAttributesexists.DocCode, DocumentAttributesexists.TxCode);
                    if (record != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DocumentAttributesExists : {0} ", ex.Message));
                return false;
            }
        }
        /// <summary>
        /// Reads the document attributesby key.
        /// </summary>
        /// <param name="DocCode">The document code.</param>
        /// <param name="TxCode">The tx code.</param>
        /// <returns>ERP_DocumentAttributes.</returns>
        public ERP_DocumentAttributes ReadDocumentAttributesbyKey(string DocCode, string TxCode)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.ERP_DocumentAttributes.Where(c => c.DocCode == DocCode && c.TxCode == TxCode).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadDocumentAttributesbyKey : {0} ", ex.Message));
                return null;
            }
        }


        /// <summary>
        /// Return all DocumentAttributes records
        /// </summary>
        /// <returns>ERP_DocumentAttributes[].</returns>
        public ERP_DocumentAttributes[] ReadAllDocumentAttributes()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.ERP_DocumentAttributes.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAllDocumentAttributes : {0} ", ex.Message));
                return new ERP_DocumentAttributes[0];
            }
        }

        /// <summary>
        /// Execute the query and Return DocumentAttributes records
        /// </summary>
        /// <returns>IQueryable&lt;ERP_DocumentAttributes&gt;.</returns>
        public IQueryable<ERP_DocumentAttributes> ReadDocumentAttributes()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.ERP_DocumentAttributes.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadDocumentAttributes : {0} ", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Update DocumentAttributes record
        /// </summary>
        /// <param name="modifiedDocumentAttributes">The modified document attributes.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateDocumentAttributes(ERP_DocumentAttributes modifiedDocumentAttributes)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        ERP_DocumentAttributes original = ctx.ERP_DocumentAttributes.Find(modifiedDocumentAttributes.DocCode, modifiedDocumentAttributes.TxCode);

                        if (original != null)
                        {
                            var document = ctx.ERP_Document.Find(modifiedDocumentAttributes.ERP_Document.DocCode);
                            modifiedDocumentAttributes.ERP_Document = document;
                            ctx.Entry(original).CurrentValues.SetValues(modifiedDocumentAttributes);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("DocumentAttributes with DocCode:{0} TxCode:{1} was not found.", modifiedDocumentAttributes.DocCode, modifiedDocumentAttributes.TxCode)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateDocumentAttributes : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Delete DocumentAttributes record
        /// </summary>
        /// <param name="deletingDocumentAttributes">The deleting document attributes.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteDocumentAttributes(ERP_DocumentAttributes deletingDocumentAttributes)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        ERP_DocumentAttributes original = ctx.ERP_DocumentAttributes.Find(deletingDocumentAttributes.DocCode, deletingDocumentAttributes.TxCode);

                        if (original != null)
                        {
                            ctx.Entry(original).State = EntityState.Deleted;
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("DocumentAttributes with DocCode:{0} TxCode:{1} was not found.", deletingDocumentAttributes.DocCode, deletingDocumentAttributes.TxCode)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteDocumentAttributes) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        #endregion

        #region DocumentAttributesBankInfo

        /// <summary>
        /// Creates the document attributes bank information.
        /// </summary>
        /// <param name="new_ERP_DocumentAttributesBankInfo">The new_ er p_ document attributes bank information.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateDocumentAttributesBankInfo(ERP_DocumentAttributesBankInfo new_ERP_DocumentAttributesBankInfo)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        var documentAttributes = ctx.ERP_DocumentAttributes.Find(new_ERP_DocumentAttributesBankInfo.ERP_DocumentAttributes.DocCode, new_ERP_DocumentAttributesBankInfo.ERP_DocumentAttributes.TxCode);
                        new_ERP_DocumentAttributesBankInfo.ERP_DocumentAttributes = documentAttributes;
                        var bankAccount = ctx.FIN_BankAccount.Find(new_ERP_DocumentAttributesBankInfo.FIN_BankAccount.BankCode, new_ERP_DocumentAttributesBankInfo.FIN_BankAccount.BankBranchCode, new_ERP_DocumentAttributesBankInfo.FIN_BankAccount.AccountSEQNo);
                        new_ERP_DocumentAttributesBankInfo.FIN_BankAccount = bankAccount;
                        ctx.ERP_DocumentAttributesBankInfo.Add(new_ERP_DocumentAttributesBankInfo);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };

            }

            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new DocumentAttributesBankInfo : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Queries the document attributes bank information.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;ERP_DocumentAttributesBankInfo&gt;.</returns>
        public ResultDTO<ERP_DocumentAttributesBankInfo> QueryDocumentAttributesBankInfo(DocumentAttributesBankInfoQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.ERP_DocumentAttributesBankInfo.AsQueryable();

                    if (query.DocCode != null)
                    {
                        c = c.Where(i => i.DocCode == query.DocCode);
                    }
                    if (query.TxCode != null)
                    {
                        c = c.Where(i => i.TxCode == query.TxCode);
                    }
                    if (query.FixedBankCode != null)
                    {
                        c = c.Where(i => i.FixedBankCode == query.FixedBankCode);
                    }
                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.DocCode).Skip((pageNumber - 1) * pageSize).Take(pageSize).Include(s => s.ERP_DocumentAttributes).Include(s => s.FIN_BankAccount).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<ERP_DocumentAttributesBankInfo>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on QueryDocumentAttributesBankInfo : {0} ", ex.Message));
                return null;
            }
        }
        /// <summary>
        /// Documents the attributes bank information exists.
        /// </summary>
        /// <param name="DocumentAttributesBankInfoexists">The document attributes bank infoexists.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool DocumentAttributesBankInfoExists(ERP_DocumentAttributesBankInfo DocumentAttributesBankInfoexists)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    ERP_DocumentAttributesBankInfo record = ctx.ERP_DocumentAttributesBankInfo.Find(DocumentAttributesBankInfoexists.DocCode, DocumentAttributesBankInfoexists.TxCode);
                    if (record != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DocumentAttributesBankInfoExists : {0} ", ex.Message));
                return false;
            }
        }
        /// <summary>
        /// Reads the document attributes bank infoby key.
        /// </summary>
        /// <param name="DocCode">The document code.</param>
        /// <param name="TxCode">The tx code.</param>
        /// <returns>ERP_DocumentAttributesBankInfo.</returns>
        public ERP_DocumentAttributesBankInfo ReadDocumentAttributesBankInfobyKey(string DocCode, string TxCode)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.ERP_DocumentAttributesBankInfo.Where(c => c.DocCode == DocCode && c.TxCode == TxCode).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadDocumentAttributesBankInfobyKey : {0} ", ex.Message));
                return null;
            }
        }


        /// <summary>
        /// Return all DocumentAttributesBankInfo records
        /// </summary>
        /// <returns>ERP_DocumentAttributesBankInfo[].</returns>
        public ERP_DocumentAttributesBankInfo[] ReadAllDocumentAttributesBankInfo()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.ERP_DocumentAttributesBankInfo.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAllDocumentAttributesBankInfo : {0} ", ex.Message));
                return new ERP_DocumentAttributesBankInfo[0];
            }
        }

        /// <summary>
        /// Execute the query and Return DocumentAttributesBankInfo records
        /// </summary>
        /// <returns>IQueryable&lt;ERP_DocumentAttributesBankInfo&gt;.</returns>
        public IQueryable<ERP_DocumentAttributesBankInfo> ReadDocumentAttributesBankInfo()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.ERP_DocumentAttributesBankInfo.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadDocumentAttributesBankInfo : {0} ", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Update DocumentAttributesBankInfo record
        /// </summary>
        /// <param name="modifiedDocumentAttributesBankInfo">The modified document attributes bank information.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateDocumentAttributesBankInfo(ERP_DocumentAttributesBankInfo modifiedDocumentAttributesBankInfo)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        ERP_DocumentAttributesBankInfo original = ctx.ERP_DocumentAttributesBankInfo.Find(modifiedDocumentAttributesBankInfo.DocCode, modifiedDocumentAttributesBankInfo.TxCode);

                        if (original != null)
                        {
                            var documentAttributes = ctx.ERP_DocumentAttributes.Find(modifiedDocumentAttributesBankInfo.ERP_DocumentAttributes.DocCode, modifiedDocumentAttributesBankInfo.ERP_DocumentAttributes.TxCode);
                            modifiedDocumentAttributesBankInfo.ERP_DocumentAttributes = documentAttributes;
                            var bankAccount = ctx.FIN_BankAccount.Find(modifiedDocumentAttributesBankInfo.FIN_BankAccount.BankCode, modifiedDocumentAttributesBankInfo.FIN_BankAccount.BankBranchCode, modifiedDocumentAttributesBankInfo.FIN_BankAccount.AccountSEQNo);
                            modifiedDocumentAttributesBankInfo.FIN_BankAccount = bankAccount;
                            ctx.Entry(original).CurrentValues.SetValues(modifiedDocumentAttributesBankInfo);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("PrimaryTransaction with DocCode:{0} TxCode:{1} was not found.", modifiedDocumentAttributesBankInfo.DocCode, modifiedDocumentAttributesBankInfo.TxCode)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateDocumentAttributesBankInfo : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Delete DocumentAttributesBankInfo record
        /// </summary>
        /// <param name="deletingDocumentAttributesBankInfo">The deleting document attributes bank information.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteDocumentAttributesBankInfo(ERP_DocumentAttributesBankInfo deletingDocumentAttributesBankInfo)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        ERP_DocumentAttributesBankInfo original = ctx.ERP_DocumentAttributesBankInfo.Find(deletingDocumentAttributesBankInfo.DocCode, deletingDocumentAttributesBankInfo.TxCode);

                        if (original != null)
                        {
                            ctx.Entry(original).State = EntityState.Deleted;
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("PrimaryTransaction with DocCode:{0} TxCode:{1} was not found.", deletingDocumentAttributesBankInfo.DocCode, deletingDocumentAttributesBankInfo.TxCode)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteDocumentAttributesBankInfo) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        #endregion

        #region DocumentAttributesRef

        /// <summary>
        /// Creates the document attributes reference.
        /// </summary>
        /// <param name="new_ERP_DocumentAttributesRef">The new_ er p_ document attributes reference.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateDocumentAttributesRef(ERP_DocumentAttributesRef new_ERP_DocumentAttributesRef)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        var documentAttributes = ctx.ERP_DocumentAttributes.Find(new_ERP_DocumentAttributesRef.ERP_DocumentAttributes.DocCode, new_ERP_DocumentAttributesRef.ERP_DocumentAttributes.TxCode);
                        new_ERP_DocumentAttributesRef.ERP_DocumentAttributes = documentAttributes;
                        ctx.ERP_DocumentAttributesRef.Add(new_ERP_DocumentAttributesRef);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };

            }

            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new DocumentAttributesRef : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Queries the document attributes reference.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;ERP_DocumentAttributesRef&gt;.</returns>
        public ResultDTO<ERP_DocumentAttributesRef> QueryDocumentAttributesRef(DocumentAttributesRefQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.ERP_DocumentAttributesRef.AsQueryable();

                    if (query.DocCode != null)
                    {
                        c = c.Where(i => i.DocCode == query.DocCode);
                    }
                    if (query.TxCode != null)
                    {
                        c = c.Where(i => i.TxCode == query.TxCode);
                    }
                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.DocCode).Skip((pageNumber - 1) * pageSize).Take(pageSize).Include(s => s.ERP_DocumentAttributes).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<ERP_DocumentAttributesRef>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on QueryDocumentAttributesRef : {0} ", ex.Message));
                return null;
            }
        }
        /// <summary>
        /// Documents the attributes reference exists.
        /// </summary>
        /// <param name="DocumentAttributesRefexists">The document attributes refexists.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool DocumentAttributesRefExists(ERP_DocumentAttributesRef DocumentAttributesRefexists)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    ERP_DocumentAttributesRef record = ctx.ERP_DocumentAttributesRef.Find(DocumentAttributesRefexists.DocCode, DocumentAttributesRefexists.TxCode, DocumentAttributesRefexists.RefNo);
                    if (record != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DocumentAttributesRefExists : {0} ", ex.Message));
                return false;
            }
        }
        /// <summary>
        /// Reads the document attributes refby key.
        /// </summary>
        /// <param name="DocCode">The document code.</param>
        /// <param name="TxCode">The tx code.</param>
        /// <param name="RefNo">The reference no.</param>
        /// <returns>ERP_DocumentAttributesRef.</returns>
        public ERP_DocumentAttributesRef ReadDocumentAttributesRefbyKey(string DocCode, string TxCode, int RefNo)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.ERP_DocumentAttributesRef.Where(c => c.DocCode == DocCode && c.TxCode == TxCode && c.RefNo == RefNo).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadDocumentAttributesRefbyKey : {0} ", ex.Message));
                return null;
            }
        }


        /// <summary>
        /// Return all DocumentAttributesRef records
        /// </summary>
        /// <returns>ERP_DocumentAttributesRef[].</returns>
        public ERP_DocumentAttributesRef[] ReadAllDocumentAttributesRef()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.ERP_DocumentAttributesRef.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAllDocumentAttributesRef : {0} ", ex.Message));
                return new ERP_DocumentAttributesRef[0];
            }
        }

        /// <summary>
        /// Execute the query and Return DocumentAttributesRef records
        /// </summary>
        /// <returns>IQueryable&lt;ERP_DocumentAttributesRef&gt;.</returns>
        public IQueryable<ERP_DocumentAttributesRef> ReadDocumentAttributesRef()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.ERP_DocumentAttributesRef.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadDocumentAttributesRef : {0} ", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Update DocumentAttributesRef record
        /// </summary>
        /// <param name="modifiedDocumentAttributesRef">The modified document attributes reference.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateDocumentAttributesRef(ERP_DocumentAttributesRef modifiedDocumentAttributesRef)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        ERP_DocumentAttributesRef original = ctx.ERP_DocumentAttributesRef.Find(modifiedDocumentAttributesRef.DocCode, modifiedDocumentAttributesRef.TxCode);

                        if (original != null)
                        {
                            var documentAttributes = ctx.ERP_DocumentAttributes.Find(modifiedDocumentAttributesRef.ERP_DocumentAttributes.DocCode, modifiedDocumentAttributesRef.ERP_DocumentAttributes.TxCode);
                            modifiedDocumentAttributesRef.ERP_DocumentAttributes = documentAttributes;
                            ctx.Entry(original).CurrentValues.SetValues(modifiedDocumentAttributesRef);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("PrimaryTransaction with DocCode:{0} TxCode:{1} was not found.", modifiedDocumentAttributesRef.DocCode, modifiedDocumentAttributesRef.TxCode)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateDocumentAttributesRef : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Delete DocumentAttributesRef record
        /// </summary>
        /// <param name="deletingDocumentAttributesRef">The deleting document attributes reference.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteDocumentAttributesRef(ERP_DocumentAttributesRef deletingDocumentAttributesRef)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        ERP_DocumentAttributesRef original = ctx.ERP_DocumentAttributesRef.Find(deletingDocumentAttributesRef.DocCode, deletingDocumentAttributesRef.TxCode);

                        if (original != null)
                        {
                            ctx.Entry(original).State = EntityState.Deleted;
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("DocumentAttributesRef with DocCode:{0} TxCode:{1} was not found.", deletingDocumentAttributesRef.DocCode, deletingDocumentAttributesRef.TxCode)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteDocumentAttributesRef) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        #endregion

        #region CostCenter
        /// <summary>
        /// Creates the cost center.
        /// </summary>
        /// <param name="new_FIN_CostCenter">The new_ fi n_ cost center.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateCostCenter(FIN_CostCenter new_FIN_CostCenter)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        ctx.FIN_CostCenter.Add(new_FIN_CostCenter);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };

            }

            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new CostCenter : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Queries the cost center.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;FIN_CostCenter&gt;.</returns>
        public ResultDTO<FIN_CostCenter> QueryCostCenter(CostCenterQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.FIN_CostCenter.AsQueryable();

                    if (query.CostCenterCode != null)
                    {
                        c = c.Where(i => i.CostCenterCode == query.CostCenterCode);
                    }
                    if (query.CostCenterDescription != null)
                    {
                        c = c.Where(i => i.CostCenterDescription == query.CostCenterDescription);
                    }
                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.CostCenterCode).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<FIN_CostCenter>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on QueryCostCenter : {0} ", ex.Message));
                return null;
            }
        }
        /// <summary>
        /// Costs the center exists.
        /// </summary>
        /// <param name="CostCenterexists">The cost centerexists.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool CostCenterExists(FIN_CostCenter CostCenterexists)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    FIN_CostCenter record = ctx.FIN_CostCenter.Find(CostCenterexists.CostCenterCode);
                    if (record != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on CostCenterExists : {0} ", ex.Message));
                return false;
            }
        }
        /// <summary>
        /// Reads the cost centerby key.
        /// </summary>
        /// <param name="CostCenterCode">The cost center code.</param>
        /// <returns>FIN_CostCenter.</returns>
        public FIN_CostCenter ReadCostCenterbyKey(string CostCenterCode)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_CostCenter.Where(c => c.CostCenterCode == CostCenterCode).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadCostCenterbyKey : {0} ", ex.Message));
                return null;
            }
        }
        /// <summary>
        /// Return all CostCenter records
        /// </summary>
        /// <returns>FIN_CostCenter[].</returns>
        public FIN_CostCenter[] ReadAllCostCenter()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_CostCenter.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAllCostCenter : {0} ", ex.Message));
                return new FIN_CostCenter[0];
            }
        }
        /// <summary>
        /// Execute the query and Return CostCenter records
        /// </summary>
        /// <returns>IQueryable&lt;FIN_CostCenter&gt;.</returns>
        public IQueryable<FIN_CostCenter> ReadCostCenter()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_CostCenter.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadCostCenter : {0} ", ex.Message));
                return null;
            }
        }
        /// <summary>
        /// Update CostCenter record
        /// </summary>
        /// <param name="modifiedCostCenter">The modified cost center.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateCostCenter(FIN_CostCenter modifiedCostCenter)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        FIN_CostCenter original = ctx.FIN_CostCenter.Find(modifiedCostCenter.CostCenterCode);

                        if (original != null)
                        {
                            ctx.Entry(original).CurrentValues.SetValues(modifiedCostCenter);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("CostCenter with CostCenterCode:{0} was not found.", modifiedCostCenter.CostCenterCode)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateCostCenter : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Delete PrimaryTransaction record
        /// </summary>
        /// <param name="deletingCostCenter">The deleting cost center.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteCostCenter(FIN_CostCenter deletingCostCenter)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        FIN_CostCenter original = ctx.FIN_CostCenter.Find(deletingCostCenter.CostCenterCode);

                        if (original != null)
                        {
                            ctx.Entry(original).State = EntityState.Deleted;
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("CostCenter with CostCenterCode:{0}  was not found.", deletingCostCenter.CostCenterCode)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteCostCenter) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        #endregion

        #region CostCenterwiseConfiguration
        /// <summary>
        /// Creates the cost centerwise configuration.
        /// </summary>
        /// <param name="new_FIN_CostCenterwiseConfiguration">The new_ fi n_ cost centerwise configuration.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateCostCenterwiseConfiguration(FIN_CostCenterwiseConfiguration new_FIN_CostCenterwiseConfiguration)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        var costCenter = ctx.FIN_CostCenter.Find(new_FIN_CostCenterwiseConfiguration.FIN_CostCenter.CostCenterCode);
                        new_FIN_CostCenterwiseConfiguration.FIN_CostCenter = costCenter;

                        var currency = ctx.FIN_Currency.Find(new_FIN_CostCenterwiseConfiguration.FIN_Currency.CurrencyCode);
                        new_FIN_CostCenterwiseConfiguration.FIN_Currency = currency;

                        var generalLedgerAccount = ctx.FIN_GeneralLedgerAccount.Find(new_FIN_CostCenterwiseConfiguration.FIN_GeneralLedgerAccount.AccountNo);
                        new_FIN_CostCenterwiseConfiguration.FIN_GeneralLedgerAccount = generalLedgerAccount;

                        var documentAttributes = ctx.ERP_DocumentAttributes.Find(new_FIN_CostCenterwiseConfiguration.ERP_DocumentAttributes.DocCode, new_FIN_CostCenterwiseConfiguration.ERP_DocumentAttributes.TxCode);
                        new_FIN_CostCenterwiseConfiguration.ERP_DocumentAttributes = documentAttributes;

                        ctx.FIN_CostCenterwiseConfiguration.Add(new_FIN_CostCenterwiseConfiguration);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };

            }

            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new CostCenterwiseConfiguration : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Reads the cost centerwise configurationby key.
        /// </summary>
        /// <param name="RevNo">The rev no.</param>
        /// <returns>FIN_CostCenterwiseConfiguration.</returns>
        public FIN_CostCenterwiseConfiguration ReadCostCenterwiseConfigurationbyKey(int RevNo)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_CostCenterwiseConfiguration.Where(c => c.RevNo == RevNo).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadCostCenterwiseConfigurationbyKey : {0} ", ex.Message));
                return null;
            }
        }
        /// <summary>
        /// Return all CostCenterwiseConfiguration records
        /// </summary>
        /// <returns>FIN_CostCenterwiseConfiguration[].</returns>
        public FIN_CostCenterwiseConfiguration[] ReadAllCostCenterwiseConfiguration()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_CostCenterwiseConfiguration.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAllCostCenterwiseConfiguration : {0} ", ex.Message));
                return new FIN_CostCenterwiseConfiguration[0];
            }
        }
        /// <summary>
        /// Reads the cost centerwise configuration.
        /// </summary>
        /// <returns>IQueryable&lt;FIN_CostCenterwiseConfiguration&gt;.</returns>
        public IQueryable<FIN_CostCenterwiseConfiguration> ReadCostCenterwiseConfiguration()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_CostCenterwiseConfiguration.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadCostCenterwiseConfiguration : {0} ", ex.Message));
                return null;
            }
        }
        /// <summary>
        /// Updates the cost centerwise configuration.
        /// </summary>
        /// <param name="modifiedCostCenterwiseConfiguration">The modified cost centerwise configuration.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateCostCenterwiseConfiguration(FIN_CostCenterwiseConfiguration modifiedCostCenterwiseConfiguration)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        FIN_CostCenterwiseConfiguration original = ctx.FIN_CostCenterwiseConfiguration.Find(modifiedCostCenterwiseConfiguration.RevNo);

                        if (original != null)
                        {
                            var costCenter = ctx.FIN_CostCenter.Find(modifiedCostCenterwiseConfiguration.FIN_CostCenter.CostCenterCode);
                            modifiedCostCenterwiseConfiguration.FIN_CostCenter = costCenter;

                            var currency = ctx.FIN_Currency.Find(modifiedCostCenterwiseConfiguration.FIN_Currency.CurrencyCode);
                            modifiedCostCenterwiseConfiguration.FIN_Currency = currency;

                            var generalLedgerAccount = ctx.FIN_GeneralLedgerAccount.Find(modifiedCostCenterwiseConfiguration.FIN_GeneralLedgerAccount.AccountNo);
                            modifiedCostCenterwiseConfiguration.FIN_GeneralLedgerAccount = generalLedgerAccount;

                            var documentAttributes = ctx.ERP_DocumentAttributes.Find(modifiedCostCenterwiseConfiguration.ERP_DocumentAttributes.DocCode, modifiedCostCenterwiseConfiguration.ERP_DocumentAttributes.TxCode);
                            modifiedCostCenterwiseConfiguration.ERP_DocumentAttributes = documentAttributes;


                            ctx.Entry(original).CurrentValues.SetValues(modifiedCostCenterwiseConfiguration);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("CostCenterwiseConfiguration with RevNo:{0}  was not found.", modifiedCostCenterwiseConfiguration.RevNo)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateCostCenterwiseConfiguration : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Deletes the cost centerwise configuration.
        /// </summary>
        /// <param name="deletingCostCenterwiseConfiguration">The deleting cost centerwise configuration.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteCostCenterwiseConfiguration(FIN_CostCenterwiseConfiguration deletingCostCenterwiseConfiguration)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        FIN_CostCenterwiseConfiguration original = ctx.FIN_CostCenterwiseConfiguration.Find(deletingCostCenterwiseConfiguration.RevNo);

                        if (original != null)
                        {
                            ctx.Entry(original).State = EntityState.Deleted;
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("CostCenterwiseConfiguration with RevNo:{0} was not found.", deletingCostCenterwiseConfiguration.RevNo)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteCostCenterwiseConfiguration) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Costs the centerwise configuration exists.
        /// </summary>
        /// <param name="existsCostCenterwiseConfiguration">The exists cost centerwise configuration.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool CostCenterwiseConfigurationExists(FIN_CostCenterwiseConfiguration existsCostCenterwiseConfiguration)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    FIN_CostCenterwiseConfiguration record = ctx.FIN_CostCenterwiseConfiguration.Find(existsCostCenterwiseConfiguration.RevNo);
                    if (record != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on CostCenterwiseConfigurationExists : {0} ", ex.Message));
                return false;
            }
        }
        /// <summary>
        /// Queries the cost centerwise configuration.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;FIN_CostCenterwiseConfiguration&gt;.</returns>
        public ResultDTO<FIN_CostCenterwiseConfiguration> QueryCostCenterwiseConfiguration(CostCenterwiseConfigurationQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.FIN_CostCenterwiseConfiguration.AsQueryable();

                    if (query.CostCenterCode != null)
                    {
                        c = c.Where(i => i.CostCenterCode == query.CostCenterCode);
                    }
                    if (query.RevNo != null)
                    {
                        c = c.Where(i => i.RevNo == query.RevNo);
                    }
                    if (query.BaseCurrencyCode != null)
                    {
                        c = c.Where(i => i.BaseCurrencyCode == query.BaseCurrencyCode);
                    }
                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.CostCenterCode).Skip((pageNumber - 1) * pageSize).Take(pageSize).Include(s => s.FIN_CostCenter).Include(s => s.FIN_Currency).Include(s => s.FIN_GeneralLedgerAccount).Include(s => s.ERP_DocumentAttributes).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<FIN_CostCenterwiseConfiguration>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on QueryCostCenterwiseConfiguration : {0} ", ex.Message));
                return null;
            }
        }
        #endregion

        #region FixedTxnAttributes

        /// <summary>
        /// Creates the fixed TXN attributes.
        /// </summary>
        /// <param name="new_ERP_FixedTxnAttributes">The new_ er p_ fixed TXN attributes.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateFixedTxnAttributes(ERP_FixedTxnAttributes new_ERP_FixedTxnAttributes)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        new_ERP_FixedTxnAttributes.FIN_CostCenter = ctx.FIN_CostCenter.Find(new_ERP_FixedTxnAttributes.FIN_CostCenter.CostCenterCode);
                        new_ERP_FixedTxnAttributes.FIN_CostCenter1 = ctx.FIN_CostCenter.Find(new_ERP_FixedTxnAttributes.FIN_CostCenter1.CostCenterCode);
                        new_ERP_FixedTxnAttributes.FIN_CostCenter2 = ctx.FIN_CostCenter.Find(new_ERP_FixedTxnAttributes.FIN_CostCenter2.CostCenterCode);
                        new_ERP_FixedTxnAttributes.FIN_CostCenter3 = ctx.FIN_CostCenter.Find(new_ERP_FixedTxnAttributes.FIN_CostCenter3.CostCenterCode);
                        new_ERP_FixedTxnAttributes.FIN_Currency = ctx.FIN_Currency.Find(new_ERP_FixedTxnAttributes.FIN_Currency.CurrencyCode);
                        new_ERP_FixedTxnAttributes.FIN_Currency1 = ctx.FIN_Currency.Find(new_ERP_FixedTxnAttributes.FIN_Currency1.CurrencyCode);
                        new_ERP_FixedTxnAttributes.FIN_Currency2 = ctx.FIN_Currency.Find(new_ERP_FixedTxnAttributes.FIN_Currency2.CurrencyCode);
                        new_ERP_FixedTxnAttributes.FIN_Currency3 = ctx.FIN_Currency.Find(new_ERP_FixedTxnAttributes.FIN_Currency3.CurrencyCode);
                        new_ERP_FixedTxnAttributes.FIN_GeneralLedgerAccount = ctx.FIN_GeneralLedgerAccount.Find(new_ERP_FixedTxnAttributes.FIN_GeneralLedgerAccount.AccountNo);
                        new_ERP_FixedTxnAttributes.FIN_GeneralLedgerAccount1 = ctx.FIN_GeneralLedgerAccount.Find(new_ERP_FixedTxnAttributes.FIN_GeneralLedgerAccount1.AccountNo);
                        new_ERP_FixedTxnAttributes.FIN_GeneralLedgerAccount2 = ctx.FIN_GeneralLedgerAccount.Find(new_ERP_FixedTxnAttributes.FIN_GeneralLedgerAccount2.AccountNo);
                        new_ERP_FixedTxnAttributes.FIN_GeneralLedgerAccount3 = ctx.FIN_GeneralLedgerAccount.Find(new_ERP_FixedTxnAttributes.FIN_GeneralLedgerAccount3.AccountNo);
                        new_ERP_FixedTxnAttributes.FIN_GeneralLedgerAccount4 = ctx.FIN_GeneralLedgerAccount.Find(new_ERP_FixedTxnAttributes.FIN_GeneralLedgerAccount4.AccountNo);
                        new_ERP_FixedTxnAttributes.ERP_DocumentAttributes = ctx.ERP_DocumentAttributes.Find(new_ERP_FixedTxnAttributes.ERP_DocumentAttributes.DocCode, new_ERP_FixedTxnAttributes.ERP_DocumentAttributes.TxCode);

                        ctx.ERP_FixedTxnAttributes.Add(new_ERP_FixedTxnAttributes);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new FixedTxnAttributes : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Queries the fixed transaction attributes.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;ERP_FixedTxnAttributes&gt;.</returns>
        public ResultDTO<ERP_FixedTxnAttributes> QueryFixedTransactionAttributes(FixedTxnAttributesQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.ERP_FixedTxnAttributes.AsQueryable();

                    if (query.DocCode != null)
                    {
                        c = c.Where(i => i.DocCode == query.DocCode);
                    }
                    if (query.TxCode != null)
                    {
                        c = c.Where(i => i.TxCode == query.TxCode);
                    }
                    if (query.GLCode != null)
                    {
                        c = c.Where(i => i.GLCode == query.GLCode);
                    }
                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.DocCode).Skip((pageNumber - 1) * pageSize).Take(pageSize)
                        .Include(s => s.FIN_CostCenter)
                        .Include(s => s.FIN_CostCenter1)
                        .Include(s => s.FIN_CostCenter2)
                        .Include(s => s.FIN_CostCenter3)
                        .Include(s => s.FIN_Currency)
                        .Include(s => s.FIN_Currency1)
                        .Include(s => s.FIN_Currency2)
                        .Include(s => s.FIN_Currency3)
                        .Include(s => s.FIN_GeneralLedgerAccount)
                        .Include(s => s.FIN_GeneralLedgerAccount1)
                        .Include(s => s.FIN_GeneralLedgerAccount2)
                        .Include(s => s.FIN_GeneralLedgerAccount3)
                        .Include(s => s.FIN_GeneralLedgerAccount4)
                        .Include(s => s.ERP_DocumentAttributes)
                        .ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<ERP_FixedTxnAttributes>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on QueryPrimaryTransaction : {0} ", ex.Message));
                return null;
            }
        }
        /// <summary>
        /// Reads the fixed TXN attributesby key.
        /// </summary>
        /// <param name="DocCode">The document code.</param>
        /// <param name="TxCode">The tx code.</param>
        /// <param name="GLCode">The gl code.</param>
        /// <returns>ERP_FixedTxnAttributes.</returns>
        public ERP_FixedTxnAttributes ReadFixedTxnAttributesbyKey(string DocCode, string TxCode, string GLCode)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.ERP_FixedTxnAttributes.Where(c => c.DocCode == DocCode && c.TxCode == TxCode && c.GLCode == GLCode).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadFixedTxnAttributesbyKey : {0} ", ex.Message));
                return null;
            }
        }


        /// <summary>
        /// Return all FixedTxnAttributes records
        /// </summary>
        /// <returns>ERP_FixedTxnAttributes[].</returns>
        public ERP_FixedTxnAttributes[] ReadAllFixedTxnAttributes()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.ERP_FixedTxnAttributes.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAllFixedTxnAttributes : {0} ", ex.Message));
                return new ERP_FixedTxnAttributes[0];
            }
        }

        /// <summary>
        /// Execute the query and Return FixedTxnAttributes records
        /// </summary>
        /// <returns>IQueryable&lt;ERP_FixedTxnAttributes&gt;.</returns>
        public IQueryable<ERP_FixedTxnAttributes> ReadFixedTxnAttributes()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.ERP_FixedTxnAttributes.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadFixedTxnAttributes : {0} ", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Update FixedTxnAttributes record
        /// </summary>
        /// <param name="modifiedFixedTxnAttributes">The modified fixed TXN attributes.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateFixedTxnAttributes(ERP_FixedTxnAttributes modifiedFixedTxnAttributes)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        ERP_FixedTxnAttributes original = ctx.ERP_FixedTxnAttributes.Find(modifiedFixedTxnAttributes.DocCode, modifiedFixedTxnAttributes.TxCode, modifiedFixedTxnAttributes.GLCode);

                        if (original != null)
                        {
                            ctx.Entry(original).CurrentValues.SetValues(modifiedFixedTxnAttributes);
                            modifiedFixedTxnAttributes.FIN_CostCenter = ctx.FIN_CostCenter.Find(modifiedFixedTxnAttributes.FIN_CostCenter.CostCenterCode);
                            modifiedFixedTxnAttributes.FIN_CostCenter1 = ctx.FIN_CostCenter.Find(modifiedFixedTxnAttributes.FIN_CostCenter1.CostCenterCode);
                            modifiedFixedTxnAttributes.FIN_CostCenter2 = ctx.FIN_CostCenter.Find(modifiedFixedTxnAttributes.FIN_CostCenter2.CostCenterCode);
                            modifiedFixedTxnAttributes.FIN_CostCenter3 = ctx.FIN_CostCenter.Find(modifiedFixedTxnAttributes.FIN_CostCenter3.CostCenterCode);
                            modifiedFixedTxnAttributes.FIN_Currency = ctx.FIN_Currency.Find(modifiedFixedTxnAttributes.FIN_Currency.CurrencyCode);
                            modifiedFixedTxnAttributes.FIN_Currency1 = ctx.FIN_Currency.Find(modifiedFixedTxnAttributes.FIN_Currency1.CurrencyCode);
                            modifiedFixedTxnAttributes.FIN_Currency2 = ctx.FIN_Currency.Find(modifiedFixedTxnAttributes.FIN_Currency2.CurrencyCode);
                            modifiedFixedTxnAttributes.FIN_Currency3 = ctx.FIN_Currency.Find(modifiedFixedTxnAttributes.FIN_Currency3.CurrencyCode);
                            modifiedFixedTxnAttributes.FIN_GeneralLedgerAccount = ctx.FIN_GeneralLedgerAccount.Find(modifiedFixedTxnAttributes.FIN_GeneralLedgerAccount.AccountNo);
                            modifiedFixedTxnAttributes.FIN_GeneralLedgerAccount1 = ctx.FIN_GeneralLedgerAccount.Find(modifiedFixedTxnAttributes.FIN_GeneralLedgerAccount1.AccountNo);
                            modifiedFixedTxnAttributes.FIN_GeneralLedgerAccount2 = ctx.FIN_GeneralLedgerAccount.Find(modifiedFixedTxnAttributes.FIN_GeneralLedgerAccount2.AccountNo);
                            modifiedFixedTxnAttributes.FIN_GeneralLedgerAccount3 = ctx.FIN_GeneralLedgerAccount.Find(modifiedFixedTxnAttributes.FIN_GeneralLedgerAccount3.AccountNo);
                            modifiedFixedTxnAttributes.FIN_GeneralLedgerAccount4 = ctx.FIN_GeneralLedgerAccount.Find(modifiedFixedTxnAttributes.FIN_GeneralLedgerAccount4.AccountNo);
                            modifiedFixedTxnAttributes.ERP_DocumentAttributes = ctx.ERP_DocumentAttributes.Find(modifiedFixedTxnAttributes.ERP_DocumentAttributes.DocCode, modifiedFixedTxnAttributes.ERP_DocumentAttributes.TxCode);

                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("FixedTxnAttributes with DocCode:{0} TxCode:{1} GLCode:{2}  was not found.", modifiedFixedTxnAttributes.DocCode, modifiedFixedTxnAttributes.TxCode, modifiedFixedTxnAttributes.GLCode)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateFixedTxnAttributes : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Delete FixedTxnAttributes record
        /// </summary>
        /// <param name="deletingFixedTxnAttributes">The deleting fixed TXN attributes.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteFixedTxnAttributes(ERP_FixedTxnAttributes deletingFixedTxnAttributes)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        ERP_FixedTxnAttributes original = ctx.ERP_FixedTxnAttributes.Find(deletingFixedTxnAttributes.DocCode, deletingFixedTxnAttributes.TxCode, deletingFixedTxnAttributes.GLCode);

                        if (original != null)
                        {
                            ctx.Entry(original).State = EntityState.Deleted;
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("FixedTxnAttributes with DocCode:{0} TxCode:{1} GLCode:{2}  was not found.", deletingFixedTxnAttributes.DocCode, deletingFixedTxnAttributes.TxCode, deletingFixedTxnAttributes.GLCode)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteFixedTxnAttributes) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Fixeds the TXN attributes exists.
        /// </summary>
        /// <param name="existsFixedTxnAttributes">The exists fixed TXN attributes.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool FixedTxnAttributesExists(ERP_FixedTxnAttributes existsFixedTxnAttributes)
        {
            try
            {

                using (var ctx = new MDMEntities())
                {
                    ERP_FixedTxnAttributes original = ctx.ERP_FixedTxnAttributes.Find(existsFixedTxnAttributes.ERP_DocumentAttributes.DocCode, existsFixedTxnAttributes.ERP_DocumentAttributes.TxCode, existsFixedTxnAttributes.FIN_GeneralLedgerAccount.AccountNo);

                    if (original != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred FixedTxnAttributes : {0} ", ex.Message));
                return false;
            }
        }
        #endregion

        #region LastTransactionInfo

        /// <summary>
        /// Queries the last transaction information.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;ERP_LastTransactionInfo&gt;.</returns>
        public ResultDTO<ERP_LastTransactionInfo> QueryLastTransactionInfo(LastTransactionInfoQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.ERP_LastTransactionInfo.AsQueryable();

                    if (query.DocCode != null)
                    {
                        c = c.Where(i => i.DocCode == query.DocCode);
                    }
                    if (query.ProductCode != null)
                    {
                        c = c.Where(i => i.ProductCode == query.ProductCode);
                    }
                    if (query.SubSystemCode != null)
                    {
                        c = c.Where(i => i.SubSystemCode == query.SubSystemCode);
                    }
                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.DocCode).Skip((pageNumber - 1) * pageSize).Take(pageSize).Include(s => s.ERP_DocumentAttributes).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<ERP_LastTransactionInfo>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on QueryPrimaryTransaction : {0} ", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Creates the last transaction information.
        /// </summary>
        /// <param name="new_ERP_LastTransactionInfo">The new_ er p_ last transaction information.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateLastTransactionInfo(ERP_LastTransactionInfo new_ERP_LastTransactionInfo)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        var docAttributes = ctx.ERP_DocumentAttributes.Find(new_ERP_LastTransactionInfo.ERP_DocumentAttributes.DocCode, new_ERP_LastTransactionInfo.ERP_DocumentAttributes.TxCode);

                        new_ERP_LastTransactionInfo.ERP_DocumentAttributes = docAttributes;
                        ctx.ERP_LastTransactionInfo.Add(new_ERP_LastTransactionInfo);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new LastTransactionInfo : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Reads the last transaction infoby key.
        /// </summary>
        /// <param name="ProductCode">The product code.</param>
        /// <param name="SubSystemCode">The sub system code.</param>
        /// <param name="DocCode">The document code.</param>
        /// <param name="TxCode">The tx code.</param>
        /// <returns>ERP_LastTransactionInfo.</returns>
        public ERP_LastTransactionInfo ReadLastTransactionInfobyKey(string ProductCode, string SubSystemCode, string DocCode, string TxCode)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.ERP_LastTransactionInfo.Where(c => c.ProductCode == ProductCode && c.SubSystemCode == SubSystemCode && c.DocCode == DocCode && c.TxCode == TxCode).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadLastTransactionInfobyKey : {0} ", ex.Message));
                return null;
            }
        }


        /// <summary>
        /// Return all LastTransactionInfo records
        /// </summary>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;ERP_LastTransactionInfo&gt;.</returns>
        public ResultDTO<ERP_LastTransactionInfo> ReadAllLastTransactionInfo(int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    int totalcount = ctx.ERP_LastTransactionInfo.Count();
                    var result = ctx.ERP_LastTransactionInfo.OrderBy(i => i.DocCode).Skip((pageNumber - 1) * pageSize).Take(pageSize).Include(s => s.ERP_DocumentAttributes).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<ERP_LastTransactionInfo>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAllLastTransactionInfo : {0} ", ex.Message));
                return new ResultDTO<ERP_LastTransactionInfo>();
            }
        }

        /// <summary>
        /// Execute the query and Return LastTransactionInfo records
        /// </summary>
        /// <returns>IQueryable&lt;ERP_LastTransactionInfo&gt;.</returns>
        public IQueryable<ERP_LastTransactionInfo> ReadLastTransactionInfo()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.ERP_LastTransactionInfo.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadLastTransactionInfo : {0} ", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Update LastTransactionInfo record
        /// </summary>
        /// <param name="modifiedLastTransactionInfo">The modified last transaction information.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateLastTransactionInfo(ERP_LastTransactionInfo modifiedLastTransactionInfo)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        ERP_LastTransactionInfo original = ctx.ERP_LastTransactionInfo.Find(modifiedLastTransactionInfo.ProductCode, modifiedLastTransactionInfo.SubSystemCode, modifiedLastTransactionInfo.DocCode, modifiedLastTransactionInfo.TxCode);

                        if (original != null)
                        {
                            ctx.Entry(original).CurrentValues.SetValues(modifiedLastTransactionInfo);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("LastTransactionInfo with ProductCode:{0} SubSystemCode:{1} DocCode:{2} TxCode:{3}  was not found.", modifiedLastTransactionInfo.ProductCode, modifiedLastTransactionInfo.SubSystemCode, modifiedLastTransactionInfo.DocCode, modifiedLastTransactionInfo.TxCode)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateLastTransactionInfo : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Delete LastTransactionInfo record
        /// </summary>
        /// <param name="deletingLastTransactionInfo">The deleting last transaction information.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteLastTransactionInfo(ERP_LastTransactionInfo deletingLastTransactionInfo)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        ERP_LastTransactionInfo original = ctx.ERP_LastTransactionInfo.Find(deletingLastTransactionInfo.ProductCode, deletingLastTransactionInfo.SubSystemCode, deletingLastTransactionInfo.ERP_DocumentAttributes.DocCode, deletingLastTransactionInfo.ERP_DocumentAttributes.TxCode);

                        if (original != null)
                        {
                            ctx.Entry(original).State = EntityState.Deleted;
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("LastTransactionInfo with ProductCode:{0} SubSystemCode:{1} DocCode:{2} TxCode:{3}  was not found.", deletingLastTransactionInfo.ProductCode, deletingLastTransactionInfo.SubSystemCode, deletingLastTransactionInfo.DocCode, deletingLastTransactionInfo.TxCode)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteLastTransactionInfo) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Lasts the transaction information exists.
        /// </summary>
        /// <param name="existsLastTransactionInfo">The exists last transaction information.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool LastTransactionInfoExists(ERP_LastTransactionInfo existsLastTransactionInfo)
        {
            try
            {

                using (var ctx = new MDMEntities())
                {
                    ERP_LastTransactionInfo original = ctx.ERP_LastTransactionInfo.Find(existsLastTransactionInfo.ProductCode, existsLastTransactionInfo.SubSystemCode, existsLastTransactionInfo.ERP_DocumentAttributes.DocCode, existsLastTransactionInfo.ERP_DocumentAttributes.TxCode);

                    if (original != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred LastTransactionInfo : {0} ", ex.Message));
                return false;
            }
        }
        #endregion

        #region AccountSubType


        /// <summary>
        /// Queries the type of the account sub.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;FIN_AccountSubType&gt;.</returns>
        public ResultDTO<FIN_AccountSubType> QueryAccountSubType(AccountSubTypeQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.FIN_AccountSubType.AsQueryable();

                    if (query.AccountType != null)
                    {
                        c = c.Where(i => i.AccountType == query.AccountType);
                    }
                    if (query.AccountSubType != null)
                    {
                        c = c.Where(i => i.AccountSubType == query.AccountSubType);
                    }

                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.AccountType).Skip((pageNumber - 1) * pageSize).Include(s => s.FIN_AccountType).Take(pageSize).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<FIN_AccountSubType>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on QueryAccountSubType : {0} ", ex.Message));
                return null;
            }
        }



        /// <summary>
        /// Creates the type of the account sub.
        /// </summary>
        /// <param name="new_FIN_AccountSubType">Type of the new_ fi n_ account sub.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateAccountSubType(FIN_AccountSubType new_FIN_AccountSubType)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        var accountType = ctx.FIN_AccountType.Find(new_FIN_AccountSubType.AccountType);
                        new_FIN_AccountSubType.FIN_AccountType = accountType;
                        ctx.FIN_AccountSubType.Add(new_FIN_AccountSubType);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new LastTransactionInfo : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Reads the account sub typeby key.
        /// </summary>
        /// <param name="AccountType">Type of the account.</param>
        /// <param name="AccountSubType">Type of the account sub.</param>
        /// <returns>FIN_AccountSubType.</returns>
        public FIN_AccountSubType ReadAccountSubTypebyKey(int AccountType, int AccountSubType)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_AccountSubType.Where(c => c.AccountType == AccountType && c.AccountSubType == AccountSubType).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAccountSubTypebyKey : {0} ", ex.Message));
                return null;
            }
        }


        /// <summary>
        /// Return all AccountSubType records
        /// </summary>
        /// <returns>FIN_AccountSubType[].</returns>
        public FIN_AccountSubType[] ReadAllAccountSubType()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_AccountSubType.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAllAccountSubType : {0} ", ex.Message));
                return new FIN_AccountSubType[0];
            }
        }

        /// <summary>
        /// Execute the query and Return AccountSubType records
        /// </summary>
        /// <returns>IQueryable&lt;FIN_AccountSubType&gt;.</returns>
        public IQueryable<FIN_AccountSubType> ReadAccountSubType()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_AccountSubType.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAccountSubType : {0} ", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Update AccountSubType record
        /// </summary>
        /// <param name="modifiedAccountSubType">Type of the modified account sub.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateAccountSubType(FIN_AccountSubType modifiedAccountSubType)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        FIN_AccountSubType original = ctx.FIN_AccountSubType.Find(modifiedAccountSubType.AccountType, modifiedAccountSubType.AccountSubType);

                        if (original != null)
                        {
                            ctx.Entry(original).CurrentValues.SetValues(modifiedAccountSubType);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("AccountSubType with AccountType:{0} AccountSubType:{1}  was not found.", modifiedAccountSubType.AccountType, modifiedAccountSubType.AccountSubType)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateAccountSubType : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Delete AccountSubType record
        /// </summary>
        /// <param name="deletingAccountSubType">Type of the deleting account sub.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteAccountSubType(FIN_AccountSubType deletingAccountSubType)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        FIN_AccountSubType original = ctx.FIN_AccountSubType.Find(deletingAccountSubType.AccountType, deletingAccountSubType.AccountSubType);

                        if (original != null)
                        {
                            ctx.Entry(original).State = EntityState.Deleted;
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("AccountSubType with AccountType:{0} AccountSubType:{1}  was not found.", deletingAccountSubType.AccountType, deletingAccountSubType.AccountSubType)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteAccountSubType) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Accounts the sub type exists.
        /// </summary>
        /// <param name="existsAccountSubType">Type of the exists account sub.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool AccountSubTypeExists(FIN_AccountSubType existsAccountSubType)
        {
            try
            {

                using (var ctx = new MDMEntities())
                {
                    FIN_AccountSubType original = ctx.FIN_AccountSubType.Find(existsAccountSubType.AccountType, existsAccountSubType.AccountSubType);

                    if (original != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred AccountSubType : {0} ", ex.Message));
                return false;
            }
        }
        #endregion

        #region AccountSubTypeCategory

        /// <summary>
        /// Queries the account sub type category.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;FIN_AccountSubTypeCategory&gt;.</returns>
        public ResultDTO<FIN_AccountSubTypeCategory> QueryAccountSubTypeCategory(AccountSubTypeCategoryQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.FIN_AccountSubTypeCategory.AsQueryable();

                    if (query.AccountType != null)
                    {
                        c = c.Where(i => i.AccountType == query.AccountType);
                    }
                    if (query.AccountSubType != null)
                    {
                        c = c.Where(i => i.AccountSubType == query.AccountSubType);
                    }
                    if (query.AccountSubTypeCategory != null)
                    {
                        c = c.Where(i => i.AccountSubCatType == query.AccountSubTypeCategory);
                    }

                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.AccountType).Skip((pageNumber - 1) * pageSize).Take(pageSize).Include(s => s.FIN_AccountSubType).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<FIN_AccountSubTypeCategory>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on QueryAccountSubTypeCategory : {0} ", ex.Message));
                return null;
            }
        }


        /// <summary>
        /// Creates the account sub type category.
        /// </summary>
        /// <param name="new_FIN_AccountSubTypeCategory">The new_ fi n_ account sub type category.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateAccountSubTypeCategory(FIN_AccountSubTypeCategory new_FIN_AccountSubTypeCategory)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        var accountSubTypes = ctx.FIN_AccountSubType.Find(new_FIN_AccountSubTypeCategory.FIN_AccountSubType.AccountType, new_FIN_AccountSubTypeCategory.FIN_AccountSubType.AccountSubType);
                        new_FIN_AccountSubTypeCategory.FIN_AccountSubType = accountSubTypes;
                        ctx.FIN_AccountSubTypeCategory.Add(new_FIN_AccountSubTypeCategory);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new AccountSubTypeCategory : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Reads the account sub type categoryby key.
        /// </summary>
        /// <param name="AccountType">Type of the account.</param>
        /// <param name="AccountSubType">Type of the account sub.</param>
        /// <param name="AccountSubCatType">Type of the account sub cat.</param>
        /// <returns>FIN_AccountSubTypeCategory.</returns>
        public FIN_AccountSubTypeCategory ReadAccountSubTypeCategorybyKey(int AccountType, int AccountSubType, int AccountSubCatType)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_AccountSubTypeCategory.Where(c => c.AccountType == AccountType && c.AccountSubType == AccountSubType && c.AccountSubCatType == AccountSubCatType).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAccountSubTypeCategorybyKey : {0} ", ex.Message));
                return null;
            }
        }


        /// <summary>
        /// Return all AccountSubTypeCategory records
        /// </summary>
        /// <returns>FIN_AccountSubTypeCategory[].</returns>
        public FIN_AccountSubTypeCategory[] ReadAllAccountSubTypeCategory()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_AccountSubTypeCategory.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAllAccountSubTypeCategory : {0} ", ex.Message));
                return new FIN_AccountSubTypeCategory[0];
            }
        }

        /// <summary>
        /// Execute the query and Return AccountSubTypeCategory records
        /// </summary>
        /// <returns>IQueryable&lt;FIN_AccountSubTypeCategory&gt;.</returns>
        public IQueryable<FIN_AccountSubTypeCategory> ReadAccountSubTypeCategory()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_AccountSubTypeCategory.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAccountSubTypeCategory : {0} ", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Update AccountSubTypeCategory record
        /// </summary>
        /// <param name="modifiedAccountSubTypeCategory">The modified account sub type category.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateAccountSubTypeCategory(FIN_AccountSubTypeCategory modifiedAccountSubTypeCategory)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        FIN_AccountSubTypeCategory original = ctx.FIN_AccountSubTypeCategory.Find(modifiedAccountSubTypeCategory.AccountType, modifiedAccountSubTypeCategory.AccountSubType, modifiedAccountSubTypeCategory.AccountSubCatType);

                        if (original != null)
                        {
                            ctx.Entry(original).CurrentValues.SetValues(modifiedAccountSubTypeCategory);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("AccountSubTypeCategory with AccountType:{0} AccountSubType:{1} AccountSubCatType:{2}  was not found.", modifiedAccountSubTypeCategory.AccountType, modifiedAccountSubTypeCategory.AccountSubType, modifiedAccountSubTypeCategory.AccountSubCatType)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateAccountSubTypeCategory : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Delete AccountSubTypeCategory record
        /// </summary>
        /// <param name="deletingAccountSubTypeCategory">The deleting account sub type category.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteAccountSubTypeCategory(FIN_AccountSubTypeCategory deletingAccountSubTypeCategory)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        FIN_AccountSubTypeCategory original = ctx.FIN_AccountSubTypeCategory.Find(deletingAccountSubTypeCategory.AccountType, deletingAccountSubTypeCategory.AccountSubType, deletingAccountSubTypeCategory.AccountSubCatType);

                        if (original != null)
                        {
                            ctx.Entry(original).State = EntityState.Deleted;
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("AccountSubTypeCategory with AccountType:{0} AccountSubType:{1} AccountSubCatType:{2}  was not found.", deletingAccountSubTypeCategory.AccountType, deletingAccountSubTypeCategory.AccountSubType, deletingAccountSubTypeCategory.AccountSubCatType)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteAccountSubTypeCategory) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Accounts the sub type category exists.
        /// </summary>
        /// <param name="existsAccountSubTypeCategory">The exists account sub type category.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool AccountSubTypeCategoryExists(FIN_AccountSubTypeCategory existsAccountSubTypeCategory)
        {
            try
            {

                using (var ctx = new MDMEntities())
                {
                    FIN_AccountSubTypeCategory original = ctx.FIN_AccountSubTypeCategory.Find(existsAccountSubTypeCategory.AccountType, existsAccountSubTypeCategory.AccountSubType, existsAccountSubTypeCategory.AccountSubCatType);

                    if (original != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred AccountSubTypeCategory : {0} ", ex.Message));
                return false;
            }
        }
        #endregion

        #region AccountType

        /// <summary>
        /// Queries the type of the account.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;FIN_AccountType&gt;.</returns>
        public ResultDTO<FIN_AccountType> QueryAccountType(AccountTypeQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.FIN_AccountType.AsQueryable();

                    if (query.AccountType != null)
                    {
                        c = c.Where(i => i.AccountType == query.AccountType);
                    }

                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.AccountType).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<FIN_AccountType>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on QueryAccountType : {0} ", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Creates the type of the account.
        /// </summary>
        /// <param name="new_FIN_AccountType">Type of the new_ fi n_ account.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateAccountType(FIN_AccountType new_FIN_AccountType)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        ctx.FIN_AccountType.Add(new_FIN_AccountType);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new AccountType : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Reads the account typeby key.
        /// </summary>
        /// <param name="AccountType">Type of the account.</param>
        /// <returns>FIN_AccountType.</returns>
        public FIN_AccountType ReadAccountTypebyKey(int AccountType)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_AccountType.Where(c => c.AccountType == AccountType).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAccountTypebyKey : {0} ", ex.Message));
                return null;
            }
        }


        /// <summary>
        /// Return all AccountType records
        /// </summary>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;FIN_AccountType&gt;.</returns>
        public ResultDTO<FIN_AccountType> ReadAllAccountType(int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    int totalcount = ctx.FIN_AccountType.Count();
                    var result = ctx.FIN_AccountType.OrderBy(i => i.AccountType).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<FIN_AccountType>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAllAccountType : {0} ", ex.Message));
                return new ResultDTO<FIN_AccountType>();
            }
        }

        /// <summary>
        /// Execute the query and Return AccountType records
        /// </summary>
        /// <returns>IQueryable&lt;FIN_AccountType&gt;.</returns>
        public IQueryable<FIN_AccountType> ReadAccountType()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_AccountType.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAccountType : {0} ", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Update AccountType record
        /// </summary>
        /// <param name="modifiedAccountType">Type of the modified account.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateAccountType(FIN_AccountType modifiedAccountType)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        FIN_AccountType original = ctx.FIN_AccountType.Find(modifiedAccountType.AccountType);

                        if (original != null)
                        {
                            ctx.Entry(original).CurrentValues.SetValues(modifiedAccountType);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("AccountType with DocCode:{0} was not found.", modifiedAccountType.AccountType)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }

            }
            catch (DbEntityValidationException dbEx)
            {
                String errorMsg = "Validation Errors were detected" + Environment.NewLine;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {

                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        errorMsg += String.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;
                    }

                }
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new AccountType : {0} ", errorMsg));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = errorMsg };

            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateAccountType : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Delete AccountType record
        /// </summary>
        /// <param name="deletingAccountType">Type of the deleting account.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteAccountType(FIN_AccountType deletingAccountType)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        FIN_AccountType original = ctx.FIN_AccountType.Find(deletingAccountType.AccountType);

                        if (original != null)
                        {
                            ctx.Entry(original).State = EntityState.Deleted;
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("AccountType with AccountType:{0}  was not found.", deletingAccountType.AccountType)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteAccountType) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Accounts the type exists.
        /// </summary>
        /// <param name="existsAccountType">Type of the exists account.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool AccountTypeExists(FIN_AccountType existsAccountType)
        {
            try
            {

                using (var ctx = new MDMEntities())
                {
                    FIN_AccountType original = ctx.FIN_AccountType.Find(existsAccountType.AccountType);

                    if (original != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred AccountType : {0} ", ex.Message));
                return false;
            }
        }
        #endregion

        #region Bank
        /// <summary>
        /// Creates the bank.
        /// </summary>
        /// <param name="new_FIN_Bank">The new_ fi n_ bank.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateBank(FIN_Bank new_FIN_Bank)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        ctx.FIN_Bank.Add(new_FIN_Bank);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new Bank : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Queries the bank.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;FIN_Bank&gt;.</returns>
        public ResultDTO<FIN_Bank> QueryBank(BankQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.FIN_Bank.AsQueryable();

                    if (query.BankCode != null)
                    {
                        c = c.Where(i => i.BankCode == query.BankCode);
                    }
                    if (query.BankName != null)
                    {
                        c = c.Where(i => i.BankName == query.BankName);
                    }

                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.BankCode).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<FIN_Bank>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on QueryPrimaryTransaction : {0} ", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Reads the bankby key.
        /// </summary>
        /// <param name="BankCode">The bank code.</param>
        /// <returns>FIN_Bank.</returns>
        public FIN_Bank ReadBankbyKey(string BankCode)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_Bank.Where(c => c.BankCode == BankCode).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadBankbyKey : {0} ", ex.Message));
                return null;
            }
        }


        /// <summary>
        /// Return all Bank records
        /// </summary>
        /// <returns>FIN_Bank[].</returns>
        public FIN_Bank[] ReadAllBank()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_Bank.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAllBank : {0} ", ex.Message));
                return new FIN_Bank[0];
            }
        }

        /// <summary>
        /// Execute the query and Return Bank records
        /// </summary>
        /// <returns>IQueryable&lt;FIN_Bank&gt;.</returns>
        public IQueryable<FIN_Bank> ReadBank()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_Bank.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadBank : {0} ", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Update Bank record
        /// </summary>
        /// <param name="modifiedBank">The modified bank.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateBank(FIN_Bank modifiedBank)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        FIN_Bank original = ctx.FIN_Bank.Find(modifiedBank.BankCode);

                        if (original != null)
                        {
                            ctx.Entry(original).CurrentValues.SetValues(modifiedBank);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("Bank with BankCode:{0}  was not found.", modifiedBank.BankCode)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateBank : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Delete Bank record
        /// </summary>
        /// <param name="deletingBank">The deleting bank.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteBank(FIN_Bank deletingBank)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        FIN_Bank original = ctx.FIN_Bank.Find(deletingBank.BankCode);

                        if (original != null)
                        {
                            ctx.Entry(original).State = EntityState.Deleted;
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("Bank with BankCode:{0}  was not found.", deletingBank.BankCode)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteBank) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Banks the exists.
        /// </summary>
        /// <param name="existsBank">The exists bank.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool BankExists(FIN_Bank existsBank)
        {
            try
            {

                using (var ctx = new MDMEntities())
                {
                    FIN_Bank original = ctx.FIN_Bank.Find(existsBank.BankCode);

                    if (original != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred Bank : {0} ", ex.Message));
                return false;
            }
        }
        #endregion

        #region BankAccount

        /// <summary>
        /// Creates the bank account.
        /// </summary>
        /// <param name="new_FIN_BankAccount">The new_ fi n_ bank account.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateBankAccount(FIN_BankAccount new_FIN_BankAccount)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        var ERP_Document = ctx.ERP_Document.Find(new_FIN_BankAccount.ERP_Document.DocCode);
                        var FIN_BankBranch = ctx.FIN_BankBranch.Find(new_FIN_BankAccount.FIN_BankBranch.BankCode, new_FIN_BankAccount.FIN_BankBranch.BankBranchCode);
                        var FIN_CostCenter = ctx.FIN_CostCenter.Find(new_FIN_BankAccount.FIN_CostCenter.CostCenterCode);
                        var FIN_Currency = ctx.FIN_Currency.Find(new_FIN_BankAccount.FIN_Currency.CurrencyCode);
                        var FIN_GeneralLedgerAccount = ctx.FIN_GeneralLedgerAccount.Find(new_FIN_BankAccount.FIN_GeneralLedgerAccount.AccountNo);

                        new_FIN_BankAccount.ERP_Document = ERP_Document;
                        new_FIN_BankAccount.FIN_BankBranch = FIN_BankBranch;
                        new_FIN_BankAccount.FIN_CostCenter = FIN_CostCenter;
                        new_FIN_BankAccount.FIN_Currency = FIN_Currency;
                        new_FIN_BankAccount.FIN_GeneralLedgerAccount = FIN_GeneralLedgerAccount;

                        ctx.FIN_BankAccount.Add(new_FIN_BankAccount);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new BankAccount : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Queries the bank account.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;FIN_BankAccount&gt;.</returns>
        public ResultDTO<FIN_BankAccount> QueryBankAccount(BankAccountQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.FIN_BankAccount.AsQueryable();

                    if (query.AccountSEQNo != null)
                    {
                        c = c.Where(i => i.AccountSEQNo == query.AccountSEQNo);
                    }
                    if (query.BankBranchCode != null)
                    {
                        c = c.Where(i => i.BankBranchCode == query.BankBranchCode);
                    }
                    if (query.BankCode != null)
                    {
                        c = c.Where(i => i.BankCode == query.BankCode);
                    }
                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.BankCode).Skip((pageNumber - 1) * pageSize).Take(pageSize)
                        .Include(s => s.ERP_Document)
                        .Include(s => s.FIN_BankBranch)
                        .Include(s => s.FIN_Currency)
                        .Include(s => s.FIN_GeneralLedgerAccount)
                        .ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<FIN_BankAccount>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on QueryPrimaryTransaction : {0} ", ex.Message));
                return null;
            }
        }
        /// <summary>
        /// Reads the bank accountby key.
        /// </summary>
        /// <param name="BankCode">The bank code.</param>
        /// <param name="BankBranchCode">The bank branch code.</param>
        /// <param name="AccountSEQNo">The account seq no.</param>
        /// <returns>FIN_BankAccount.</returns>
        public FIN_BankAccount ReadBankAccountbyKey(string BankCode, string BankBranchCode, int AccountSEQNo)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_BankAccount.Where(c => c.BankCode == BankCode && c.BankBranchCode == BankBranchCode && c.AccountSEQNo == AccountSEQNo).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadBankAccountbyKey : {0} ", ex.Message));
                return null;
            }
        }


        /// <summary>
        /// Return all BankAccount records
        /// </summary>
        /// <returns>FIN_BankAccount[].</returns>
        public FIN_BankAccount[] ReadAllBankAccount()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_BankAccount.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAllBankAccount : {0} ", ex.Message));
                return new FIN_BankAccount[0];
            }
        }

        /// <summary>
        /// Execute the query and Return BankAccount records
        /// </summary>
        /// <returns>IQueryable&lt;FIN_BankAccount&gt;.</returns>
        public IQueryable<FIN_BankAccount> ReadBankAccount()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_BankAccount.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadBankAccount : {0} ", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Update BankAccount record
        /// </summary>
        /// <param name="modifiedBankAccount">The modified bank account.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateBankAccount(FIN_BankAccount modifiedBankAccount)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {

                        var ERP_Document = ctx.ERP_Document.Find(modifiedBankAccount.ERP_Document.DocCode);
                        var FIN_BankBranch = ctx.FIN_BankBranch.Find(modifiedBankAccount.FIN_BankBranch.BankCode, modifiedBankAccount.FIN_BankBranch.BankBranchCode);
                        var FIN_CostCenter = ctx.FIN_CostCenter.Find(modifiedBankAccount.FIN_CostCenter.CostCenterCode);
                        var FIN_Currency = ctx.FIN_Currency.Find(modifiedBankAccount.FIN_Currency.CurrencyCode);
                        var FIN_GeneralLedgerAccount = ctx.FIN_GeneralLedgerAccount.Find(modifiedBankAccount.FIN_GeneralLedgerAccount.AccountNo);

                        modifiedBankAccount.ERP_Document = ERP_Document;
                        modifiedBankAccount.FIN_BankBranch = FIN_BankBranch;
                        modifiedBankAccount.FIN_CostCenter = FIN_CostCenter;
                        modifiedBankAccount.FIN_Currency = FIN_Currency;
                        modifiedBankAccount.FIN_GeneralLedgerAccount = FIN_GeneralLedgerAccount;

                        FIN_BankAccount original = ctx.FIN_BankAccount.Find(modifiedBankAccount.FIN_BankBranch.BankCode, modifiedBankAccount.FIN_BankBranch.BankBranchCode, modifiedBankAccount.AccountSEQNo);

                        if (original != null)
                        {
                            ctx.Entry(original).CurrentValues.SetValues(modifiedBankAccount);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("BankAccount with BankCode:{0} BankBranchCode:{1} AccountSEQNo:{2}  was not found.", modifiedBankAccount.BankCode, modifiedBankAccount.BankBranchCode, modifiedBankAccount.AccountSEQNo)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateBankAccount : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Delete BankAccount record
        /// </summary>
        /// <param name="deletingBankAccount">The deleting bank account.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteBankAccount(FIN_BankAccount deletingBankAccount)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        FIN_BankAccount original = ctx.FIN_BankAccount.Find(deletingBankAccount.BankCode, deletingBankAccount.BankBranchCode, deletingBankAccount.AccountSEQNo);

                        if (original != null)
                        {
                            ctx.Entry(original).State = EntityState.Deleted;
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("BankAccount with BankCode:{0} BankBranchCode:{1} AccountSEQNo:{2}  was not found.", deletingBankAccount.BankCode, deletingBankAccount.BankBranchCode, deletingBankAccount.AccountSEQNo)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteBankAccount) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Banks the account exists.
        /// </summary>
        /// <param name="existsBankAccount">The exists bank account.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool BankAccountExists(FIN_BankAccount existsBankAccount)
        {
            try
            {

                using (var ctx = new MDMEntities())
                {
                    FIN_BankAccount original = ctx.FIN_BankAccount.Find(existsBankAccount.FIN_BankBranch.BankCode, existsBankAccount.FIN_BankBranch.BankBranchCode, existsBankAccount.AccountSEQNo);

                    if (original != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred BankAccount : {0} ", ex.Message));
                return false;
            }
        }
        #endregion

        #region BankBranch

        /// <summary>
        /// Creates the bank branch.
        /// </summary>
        /// <param name="new_FIN_BankBranch">The new_ fi n_ bank branch.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateBankBranch(FIN_BankBranch new_FIN_BankBranch)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        FIN_Area area = ctx.FIN_Area.Find(new_FIN_BankBranch.FIN_Area.AreaCode, new_FIN_BankBranch.FIN_Area.RegionCode);
                        FIN_Bank bank = ctx.FIN_Bank.Find(new_FIN_BankBranch.FIN_Bank.BankCode);

                        new_FIN_BankBranch.FIN_Area = area;
                        new_FIN_BankBranch.FIN_Bank = bank;

                        ctx.FIN_BankBranch.Add(new_FIN_BankBranch);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new BankBranch : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Queries the bank branch.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;FIN_BankBranch&gt;.</returns>
        public ResultDTO<FIN_BankBranch> QueryBankBranch(BankBranchQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.FIN_BankBranch.AsQueryable();

                    if (query.BankCode != null)
                    {
                        c = c.Where(i => i.BankCode == query.BankCode);
                    }
                    if (query.BankBranchCode != null)
                    {
                        c = c.Where(i => i.BankBranchCode == query.BankBranchCode);
                    }
                    if (query.BranchName != null)
                    {
                        c = c.Where(i => i.BranchName.Contains(query.BranchName));
                    }

                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.BankCode).Skip((pageNumber - 1) * pageSize).Take(pageSize)
                        .Include(s => s.FIN_Bank)
                        .Include(s => s.FIN_Area)
                        .ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<FIN_BankBranch>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on QueryPrimaryTransaction : {0} ", ex.Message));
                return null;
            }
        }
        /// <summary>
        /// Reads the bank branchby key.
        /// </summary>
        /// <param name="BankCode">The bank code.</param>
        /// <param name="BankBranchCode">The bank branch code.</param>
        /// <returns>FIN_BankBranch.</returns>
        public FIN_BankBranch ReadBankBranchbyKey(string BankCode, string BankBranchCode)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_BankBranch.Where(c => c.BankCode == BankCode && c.BankBranchCode == BankBranchCode).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadBankBranchbyKey : {0} ", ex.Message));
                return null;
            }
        }


        /// <summary>
        /// Return all BankBranch records
        /// </summary>
        /// <returns>FIN_BankBranch[].</returns>
        public FIN_BankBranch[] ReadAllBankBranch()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_BankBranch.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAllBankBranch : {0} ", ex.Message));
                return new FIN_BankBranch[0];
            }
        }

        /// <summary>
        /// Execute the query and Return BankBranch records
        /// </summary>
        /// <returns>IQueryable&lt;FIN_BankBranch&gt;.</returns>
        public IQueryable<FIN_BankBranch> ReadBankBranch()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_BankBranch.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadBankBranch : {0} ", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Update BankBranch record
        /// </summary>
        /// <param name="modifiedBankBranch">The modified bank branch.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateBankBranch(FIN_BankBranch modifiedBankBranch)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        FIN_Area area = ctx.FIN_Area.Find(modifiedBankBranch.FIN_Area.AreaCode, modifiedBankBranch.FIN_Area.RegionCode);
                        FIN_Bank bank = ctx.FIN_Bank.Find(modifiedBankBranch.FIN_Bank.BankCode);

                        modifiedBankBranch.FIN_Area = area;
                        modifiedBankBranch.FIN_Bank = bank;

                        FIN_BankBranch original = ctx.FIN_BankBranch.Find(modifiedBankBranch.BankCode, modifiedBankBranch.BankBranchCode);

                        if (original != null)
                        {
                            ctx.Entry(original).CurrentValues.SetValues(modifiedBankBranch);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("BankBranch with BankCode:{0} BankBranchCode:{1}  was not found.", modifiedBankBranch.BankCode, modifiedBankBranch.BankBranchCode)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateBankBranch : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Delete BankBranch record
        /// </summary>
        /// <param name="deletingBankBranch">The deleting bank branch.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteBankBranch(FIN_BankBranch deletingBankBranch)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        FIN_BankBranch original = ctx.FIN_BankBranch.Find(deletingBankBranch.FIN_Bank.BankCode, deletingBankBranch.BankBranchCode);

                        if (original != null)
                        {
                            ctx.Entry(original).State = EntityState.Deleted;
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("BankBranch with BankCode:{0} BankBranchCode:{1}  was not found.", deletingBankBranch.BankCode, deletingBankBranch.BankBranchCode)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteBankBranch) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Banks the branch exists.
        /// </summary>
        /// <param name="existsBankBranch">The exists bank branch.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool BankBranchExists(FIN_BankBranch existsBankBranch)
        {
            try
            {

                using (var ctx = new MDMEntities())
                {
                    FIN_BankBranch original = ctx.FIN_BankBranch.Find(existsBankBranch.FIN_Bank.BankCode, existsBankBranch.BankBranchCode);

                    if (original != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred BankBranch : {0} ", ex.Message));
                return false;
            }
        }
        #endregion

        #region ControledTransaction

        /// <summary>
        /// Queries the controlled transaction.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;FIN_ControledTransaction&gt;.</returns>
        public ResultDTO<FIN_ControledTransaction> QueryControlledTransaction(ControlledTransactionQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.FIN_ControledTransaction.AsQueryable();

                    if (query.DocCode != null)
                    {
                        c = c.Where(i => i.DocCode == query.DocCode);
                    }
                    if (query.TxnCode != null)
                    {
                        c = c.Where(i => i.TxCode == query.TxnCode);
                    }
                    if (query.GLAccountCode != null)
                    {
                        c = c.Where(i => i.GLAccountNo == query.GLAccountCode);
                    }
                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.DocCode).Skip((pageNumber - 1) * pageSize).Take(pageSize)
                        .Include(s => s.ERP_DocumentAttributes)
                        .Include(s => s.FIN_GeneralLedgerAccount)
                        .Include(s => s.FIN_Currency)
                        .Include(s => s.FIN_CostCenter)
                        .ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<FIN_ControledTransaction>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on QueryPrimaryTransaction : {0} ", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Creates the controled transaction.
        /// </summary>
        /// <param name="new_FIN_ControledTransaction">The new_ fi n_ controled transaction.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateControledTransaction(FIN_ControledTransaction new_FIN_ControledTransaction)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        var docAttributes = ctx.ERP_DocumentAttributes.Find(new_FIN_ControledTransaction.ERP_DocumentAttributes.DocCode, new_FIN_ControledTransaction.ERP_DocumentAttributes.TxCode);
                        var GLAccount = ctx.FIN_GeneralLedgerAccount.Find(new_FIN_ControledTransaction.FIN_GeneralLedgerAccount.AccountNo);
                        var currency = ctx.FIN_Currency.Find(new_FIN_ControledTransaction.FIN_Currency.CurrencyCode);
                        var costCenter = ctx.FIN_CostCenter.Find(new_FIN_ControledTransaction.FIN_CostCenter.CostCenterCode);

                        new_FIN_ControledTransaction.ERP_DocumentAttributes = docAttributes;
                        new_FIN_ControledTransaction.FIN_GeneralLedgerAccount = GLAccount;
                        new_FIN_ControledTransaction.FIN_Currency = currency;
                        new_FIN_ControledTransaction.FIN_CostCenter = costCenter;

                        ctx.FIN_ControledTransaction.Add(new_FIN_ControledTransaction);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new ControledTransaction : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Reads the controled transactionby key.
        /// </summary>
        /// <param name="DocCode">The document code.</param>
        /// <param name="TxCode">The tx code.</param>
        /// <param name="GLAccountNo">The gl account no.</param>
        /// <param name="CostCenterCode">The cost center code.</param>
        /// <param name="CurrencyCode">The currency code.</param>
        /// <returns>FIN_ControledTransaction.</returns>
        public FIN_ControledTransaction ReadControledTransactionbyKey(string DocCode, string TxCode, string GLAccountNo, string CostCenterCode, string CurrencyCode)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_ControledTransaction.Where(c => c.DocCode == DocCode && c.TxCode == TxCode && c.GLAccountNo == GLAccountNo && c.CostCenterCode == CostCenterCode && c.CurrencyCode == CurrencyCode).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadControledTransactionbyKey : {0} ", ex.Message));
                return null;
            }
        }


        /// <summary>
        /// Return all ControledTransaction records
        /// </summary>
        /// <returns>FIN_ControledTransaction[].</returns>
        public FIN_ControledTransaction[] ReadAllControledTransaction()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_ControledTransaction.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAllControledTransaction : {0} ", ex.Message));
                return new FIN_ControledTransaction[0];
            }
        }

        /// <summary>
        /// Execute the query and Return ControledTransaction records
        /// </summary>
        /// <returns>IQueryable&lt;FIN_ControledTransaction&gt;.</returns>
        public IQueryable<FIN_ControledTransaction> ReadControledTransaction()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_ControledTransaction.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadControledTransaction : {0} ", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Update ControledTransaction record
        /// </summary>
        /// <param name="modifiedControledTransaction">The modified controled transaction.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateControledTransaction(FIN_ControledTransaction modifiedControledTransaction)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        FIN_ControledTransaction original = ctx.FIN_ControledTransaction.Find(modifiedControledTransaction.ERP_DocumentAttributes.DocCode, modifiedControledTransaction.ERP_DocumentAttributes.TxCode, modifiedControledTransaction.FIN_GeneralLedgerAccount.AccountNo, modifiedControledTransaction.FIN_CostCenter.CostCenterCode, modifiedControledTransaction.FIN_Currency.CurrencyCode);

                        if (original != null)
                        {
                            ctx.Entry(original).CurrentValues.SetValues(modifiedControledTransaction);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("ControledTransaction with DocCode:{0} TxCode:{1} GLAccountNo:{2} CostCenterCode:{3} CurrencyCode:{4}  was not found.", modifiedControledTransaction.DocCode, modifiedControledTransaction.TxCode, modifiedControledTransaction.GLAccountNo, modifiedControledTransaction.CostCenterCode, modifiedControledTransaction.CurrencyCode)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateControledTransaction : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Delete ControledTransaction record
        /// </summary>
        /// <param name="deletingControledTransaction">The deleting controled transaction.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteControledTransaction(FIN_ControledTransaction deletingControledTransaction)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        FIN_ControledTransaction original = ctx.FIN_ControledTransaction.Find(deletingControledTransaction.ERP_DocumentAttributes.DocCode, deletingControledTransaction.ERP_DocumentAttributes.TxCode, deletingControledTransaction.FIN_GeneralLedgerAccount.AccountNo, deletingControledTransaction.FIN_CostCenter.CostCenterCode, deletingControledTransaction.FIN_Currency.CurrencyCode);

                        if (original != null)
                        {
                            ctx.Entry(original).State = EntityState.Deleted;
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("ControledTransaction with DocCode:{0} TxCode:{1} GLAccountNo:{2} CostCenterCode:{3} CurrencyCode:{4}  was not found.", deletingControledTransaction.DocCode, deletingControledTransaction.TxCode, deletingControledTransaction.GLAccountNo, deletingControledTransaction.CostCenterCode, deletingControledTransaction.CurrencyCode)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteControledTransaction) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Controleds the transaction exists.
        /// </summary>
        /// <param name="existsControledTransaction">The exists controled transaction.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool ControledTransactionExists(FIN_ControledTransaction existsControledTransaction)
        {
            try
            {

                using (var ctx = new MDMEntities())
                {
                    FIN_ControledTransaction original = ctx.FIN_ControledTransaction.Find(existsControledTransaction.ERP_DocumentAttributes.DocCode, existsControledTransaction.ERP_DocumentAttributes.TxCode, existsControledTransaction.FIN_GeneralLedgerAccount.AccountNo, existsControledTransaction.FIN_CostCenter.CostCenterCode, existsControledTransaction.FIN_Currency.CurrencyCode);

                    if (original != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred ControledTransaction : {0} ", ex.Message));
                return false;
            }
        }
        #endregion

        #region CustomerSupplierInfo

        /// <summary>
        /// Creates the customer supplier information.
        /// </summary>
        /// <param name="new_FIN_CustomerSupplier_Info">The new_ fi n_ customer supplier_ information.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateCustomerSupplierInfo(FIN_CustomerSupplier_Info new_FIN_CustomerSupplier_Info)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        ctx.FIN_CustomerSupplier_Info.Add(new_FIN_CustomerSupplier_Info);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new CustomerSupplierInfo : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Reads the customer supplier infoby key.
        /// </summary>
        /// <param name="CustSupFlag">The customer sup flag.</param>
        /// <param name="CusSupCode">The cus sup code.</param>
        /// <returns>FIN_CustomerSupplier_Info.</returns>
        public FIN_CustomerSupplier_Info ReadCustomerSupplierInfobyKey(int CustSupFlag, string CusSupCode)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_CustomerSupplier_Info.Where(c => c.CustSupFlag == CustSupFlag && c.CusSupCode == CusSupCode).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadCustomerSupplierInfobyKey : {0} ", ex.Message));
                return null;
            }
        }


        /// <summary>
        /// Return all CustomerSupplierInfo records
        /// </summary>
        /// <returns>FIN_CustomerSupplier_Info[].</returns>
        public FIN_CustomerSupplier_Info[] ReadAllCustomerSupplierInfo()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_CustomerSupplier_Info.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAllCustomerSupplierInfo : {0} ", ex.Message));
                return new FIN_CustomerSupplier_Info[0];
            }
        }

        /// <summary>
        /// Execute the query and Return CustomerSupplierInfo records
        /// </summary>
        /// <returns>IQueryable&lt;FIN_CustomerSupplier_Info&gt;.</returns>
        public IQueryable<FIN_CustomerSupplier_Info> ReadCustomerSupplierInfo()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_CustomerSupplier_Info.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadCustomerSupplierInfo : {0} ", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Update CustomerSupplierInfo record
        /// </summary>
        /// <param name="modifiedCustomerSupplierInfo">The modified customer supplier information.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateCustomerSupplierInfo(FIN_CustomerSupplier_Info modifiedCustomerSupplierInfo)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        FIN_CustomerSupplier_Info original = ctx.FIN_CustomerSupplier_Info.Find(modifiedCustomerSupplierInfo.CustSupFlag, modifiedCustomerSupplierInfo.CusSupCode);

                        if (original != null)
                        {
                            ctx.Entry(original).CurrentValues.SetValues(modifiedCustomerSupplierInfo);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("CustomerSupplierInfo with CustSupFlag:{0} CusSupCode:{1}  was not found.", modifiedCustomerSupplierInfo.CustSupFlag, modifiedCustomerSupplierInfo.CusSupCode)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateCustomerSupplierInfo : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Delete CustomerSupplierInfo record
        /// </summary>
        /// <param name="deletingCustomerSupplierInfo">The deleting customer supplier information.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteCustomerSupplierInfo(FIN_CustomerSupplier_Info deletingCustomerSupplierInfo)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        FIN_CustomerSupplier_Info original = ctx.FIN_CustomerSupplier_Info.Find(deletingCustomerSupplierInfo.CustSupFlag, deletingCustomerSupplierInfo.CusSupCode);

                        if (original != null)
                        {
                            ctx.Entry(original).State = EntityState.Deleted;
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("CustomerSupplierInfo with CustSupFlag:{0} CusSupCode:{1}  was not found.", deletingCustomerSupplierInfo.CustSupFlag, deletingCustomerSupplierInfo.CusSupCode)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteCustomerSupplierInfo) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Customers the supplier information exists.
        /// </summary>
        /// <param name="existsCustomerSupplierInfo">The exists customer supplier information.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool CustomerSupplierInfoExists(FIN_CustomerSupplier_Info existsCustomerSupplierInfo)
        {
            try
            {

                using (var ctx = new MDMEntities())
                {
                    FIN_CustomerSupplier_Info original = ctx.FIN_CustomerSupplier_Info.Find(existsCustomerSupplierInfo.CustSupFlag, existsCustomerSupplierInfo.CusSupCode);

                    if (original != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred CustomerSupplierInfo : {0} ", ex.Message));
                return false;
            }
        }
        #endregion

        #region CustomerSupplierBank

        /// <summary>
        /// Creates the customer supplier bank.
        /// </summary>
        /// <param name="new_FIN_CustomerSupplierBank">The new_ fi n_ customer supplier bank.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateCustomerSupplierBank(FIN_CustomerSupplierBank new_FIN_CustomerSupplierBank)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        ctx.FIN_CustomerSupplierBank.Add(new_FIN_CustomerSupplierBank);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new CustomerSupplierBank : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Reads the customer supplier bankby key.
        /// </summary>
        /// <param name="CusSupCode">The cus sup code.</param>
        /// <param name="BankCode">The bank code.</param>
        /// <param name="BranchCode">The branch code.</param>
        /// <returns>FIN_CustomerSupplierBank.</returns>
        public FIN_CustomerSupplierBank ReadCustomerSupplierBankbyKey(string CusSupCode, string BankCode, string BranchCode)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_CustomerSupplierBank.Where(c => c.CusSupCode == CusSupCode && c.BankCode == BankCode && c.BranchCode == BranchCode).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadCustomerSupplierBankbyKey : {0} ", ex.Message));
                return null;
            }
        }


        /// <summary>
        /// Return all CustomerSupplierBank records
        /// </summary>
        /// <returns>FIN_CustomerSupplierBank[].</returns>
        public FIN_CustomerSupplierBank[] ReadAllCustomerSupplierBank()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_CustomerSupplierBank.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAllCustomerSupplierBank : {0} ", ex.Message));
                return new FIN_CustomerSupplierBank[0];
            }
        }

        /// <summary>
        /// Execute the query and Return CustomerSupplierBank records
        /// </summary>
        /// <returns>IQueryable&lt;FIN_CustomerSupplierBank&gt;.</returns>
        public IQueryable<FIN_CustomerSupplierBank> ReadCustomerSupplierBank()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_CustomerSupplierBank.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadCustomerSupplierBank : {0} ", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Update CustomerSupplierBank record
        /// </summary>
        /// <param name="modifiedCustomerSupplierBank">The modified customer supplier bank.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateCustomerSupplierBank(FIN_CustomerSupplierBank modifiedCustomerSupplierBank)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        FIN_CustomerSupplierBank original = ctx.FIN_CustomerSupplierBank.Find(modifiedCustomerSupplierBank.CusSupCode, modifiedCustomerSupplierBank.BankCode, modifiedCustomerSupplierBank.BranchCode);

                        if (original != null)
                        {
                            ctx.Entry(original).CurrentValues.SetValues(modifiedCustomerSupplierBank);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("CustomerSupplierBank with CusSupCode:{0} BankCode:{1} BranchCode:{2}  was not found.", modifiedCustomerSupplierBank.CusSupCode, modifiedCustomerSupplierBank.BankCode, modifiedCustomerSupplierBank.BranchCode)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateCustomerSupplierBank : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Delete CustomerSupplierBank record
        /// </summary>
        /// <param name="deletingCustomerSupplierBank">The deleting customer supplier bank.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteCustomerSupplierBank(FIN_CustomerSupplierBank deletingCustomerSupplierBank)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        FIN_CustomerSupplierBank original = ctx.FIN_CustomerSupplierBank.Find(deletingCustomerSupplierBank.CusSupCode, deletingCustomerSupplierBank.BankCode, deletingCustomerSupplierBank.BranchCode);

                        if (original != null)
                        {
                            ctx.Entry(original).State = EntityState.Deleted;
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("CustomerSupplierBank with CusSupCode:{0} BankCode:{1} BranchCode:{2}  was not found.", deletingCustomerSupplierBank.CusSupCode, deletingCustomerSupplierBank.BankCode, deletingCustomerSupplierBank.BranchCode)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteCustomerSupplierBank) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Customers the supplier bank exists.
        /// </summary>
        /// <param name="existsCustomerSupplierBank">The exists customer supplier bank.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool CustomerSupplierBankExists(FIN_CustomerSupplierBank existsCustomerSupplierBank)
        {
            try
            {

                using (var ctx = new MDMEntities())
                {
                    FIN_CustomerSupplierBank original = ctx.FIN_CustomerSupplierBank.Find(existsCustomerSupplierBank.CusSupCode, existsCustomerSupplierBank.BankCode, existsCustomerSupplierBank.BranchCode);

                    if (original != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred CustomerSupplierBank : {0} ", ex.Message));
                return false;
            }
        }
        #endregion

        #region CustSupPeriodBalance

        /// <summary>
        /// Creates the customer sup period balance.
        /// </summary>
        /// <param name="new_FIN_CustSupPeriodBalance">The new_ fi n_ customer sup period balance.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateCustSupPeriodBalance(FIN_CustSupPeriodBalance new_FIN_CustSupPeriodBalance)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        ctx.FIN_CustSupPeriodBalance.Add(new_FIN_CustSupPeriodBalance);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new CustSupPeriodBalance : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Reads the customer sup period balanceby key.
        /// </summary>
        /// <param name="CustSupFlag">The customer sup flag.</param>
        /// <param name="CusSupCode">The cus sup code.</param>
        /// <param name="FinancialYear">The financial year.</param>
        /// <param name="AccountingPeriod">The accounting period.</param>
        /// <returns>FIN_CustSupPeriodBalance.</returns>
        public FIN_CustSupPeriodBalance ReadCustSupPeriodBalancebyKey(int CustSupFlag, string CusSupCode, int FinancialYear, int AccountingPeriod)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_CustSupPeriodBalance.Where(c => c.CustSupFlag == CustSupFlag && c.CusSupCode == CusSupCode && c.FinancialYear == FinancialYear && c.AccountingPeriod == AccountingPeriod).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadCustSupPeriodBalancebyKey : {0} ", ex.Message));
                return null;
            }
        }


        /// <summary>
        /// Return all CustSupPeriodBalance records
        /// </summary>
        /// <returns>FIN_CustSupPeriodBalance[].</returns>
        public FIN_CustSupPeriodBalance[] ReadAllCustSupPeriodBalance()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_CustSupPeriodBalance.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAllCustSupPeriodBalance : {0} ", ex.Message));
                return new FIN_CustSupPeriodBalance[0];
            }
        }

        /// <summary>
        /// Execute the query and Return CustSupPeriodBalance records
        /// </summary>
        /// <returns>IQueryable&lt;FIN_CustSupPeriodBalance&gt;.</returns>
        public IQueryable<FIN_CustSupPeriodBalance> ReadCustSupPeriodBalance()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_CustSupPeriodBalance.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadCustSupPeriodBalance : {0} ", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Update CustSupPeriodBalance record
        /// </summary>
        /// <param name="modifiedCustSupPeriodBalance">The modified customer sup period balance.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateCustSupPeriodBalance(FIN_CustSupPeriodBalance modifiedCustSupPeriodBalance)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        FIN_CustSupPeriodBalance original = ctx.FIN_CustSupPeriodBalance.Find(modifiedCustSupPeriodBalance.CustSupFlag, modifiedCustSupPeriodBalance.CusSupCode, modifiedCustSupPeriodBalance.FinancialYear, modifiedCustSupPeriodBalance.AccountingPeriod);

                        if (original != null)
                        {
                            ctx.Entry(original).CurrentValues.SetValues(modifiedCustSupPeriodBalance);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("CustSupPeriodBalance with CustSupFlag:{0} CusSupCode:{1} FinancialYear:{2} AccountingPeriod:{3}  was not found.", modifiedCustSupPeriodBalance.CustSupFlag, modifiedCustSupPeriodBalance.CusSupCode, modifiedCustSupPeriodBalance.FinancialYear, modifiedCustSupPeriodBalance.AccountingPeriod)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateCustSupPeriodBalance : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Delete CustSupPeriodBalance record
        /// </summary>
        /// <param name="deletingCustSupPeriodBalance">The deleting customer sup period balance.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteCustSupPeriodBalance(FIN_CustSupPeriodBalance deletingCustSupPeriodBalance)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        FIN_CustSupPeriodBalance original = ctx.FIN_CustSupPeriodBalance.Find(deletingCustSupPeriodBalance.CustSupFlag, deletingCustSupPeriodBalance.CusSupCode, deletingCustSupPeriodBalance.FinancialYear, deletingCustSupPeriodBalance.AccountingPeriod);

                        if (original != null)
                        {
                            ctx.Entry(original).State = EntityState.Deleted;
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("CustSupPeriodBalance with CustSupFlag:{0} CusSupCode:{1} FinancialYear:{2} AccountingPeriod:{3}  was not found.", deletingCustSupPeriodBalance.CustSupFlag, deletingCustSupPeriodBalance.CusSupCode, deletingCustSupPeriodBalance.FinancialYear, deletingCustSupPeriodBalance.AccountingPeriod)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteCustSupPeriodBalance) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Customers the sup period balance exists.
        /// </summary>
        /// <param name="existsCustSupPeriodBalance">The exists customer sup period balance.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool CustSupPeriodBalanceExists(FIN_CustSupPeriodBalance existsCustSupPeriodBalance)
        {
            try
            {

                using (var ctx = new MDMEntities())
                {
                    FIN_CustSupPeriodBalance original = ctx.FIN_CustSupPeriodBalance.Find(existsCustSupPeriodBalance.CustSupFlag, existsCustSupPeriodBalance.CusSupCode, existsCustSupPeriodBalance.FinancialYear, existsCustSupPeriodBalance.AccountingPeriod);

                    if (original != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred CustSupPeriodBalance : {0} ", ex.Message));
                return false;
            }
        }
        #endregion

        #region GeneralLedgerSummary

        /// <summary>
        /// Creates the general ledger summary.
        /// </summary>
        /// <param name="new_FIN_GeneralLedgerSummary">The new_ fi n_ general ledger summary.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateGeneralLedgerSummary(FIN_GeneralLedgerSummary new_FIN_GeneralLedgerSummary)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        ctx.FIN_GeneralLedgerSummary.Add(new_FIN_GeneralLedgerSummary);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new GeneralLedgerSummary : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Reads the general ledger summaryby key.
        /// </summary>
        /// <param name="AccountNo">The account no.</param>
        /// <param name="CurrencyCode">The currency code.</param>
        /// <param name="FinancialYear">The financial year.</param>
        /// <param name="AccountingPeriod">The accounting period.</param>
        /// <returns>FIN_GeneralLedgerSummary.</returns>
        public FIN_GeneralLedgerSummary ReadGeneralLedgerSummarybyKey(string AccountNo, string CurrencyCode, int FinancialYear, short AccountingPeriod)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_GeneralLedgerSummary.Where(c => c.AccountNo == AccountNo && c.CurrencyCode == CurrencyCode && c.FinancialYear == FinancialYear && c.AccountingPeriod == AccountingPeriod).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadGeneralLedgerSummarybyKey : {0} ", ex.Message));
                return null;
            }
        }


        /// <summary>
        /// Return all GeneralLedgerSummary records
        /// </summary>
        /// <returns>FIN_GeneralLedgerSummary[].</returns>
        public FIN_GeneralLedgerSummary[] ReadAllGeneralLedgerSummary()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_GeneralLedgerSummary.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAllGeneralLedgerSummary : {0} ", ex.Message));
                return new FIN_GeneralLedgerSummary[0];
            }
        }

        /// <summary>
        /// Execute the query and Return GeneralLedgerSummary records
        /// </summary>
        /// <returns>IQueryable&lt;FIN_GeneralLedgerSummary&gt;.</returns>
        public IQueryable<FIN_GeneralLedgerSummary> ReadGeneralLedgerSummary()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_GeneralLedgerSummary.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadGeneralLedgerSummary : {0} ", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Update GeneralLedgerSummary record
        /// </summary>
        /// <param name="modifiedGeneralLedgerSummary">The modified general ledger summary.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateGeneralLedgerSummary(FIN_GeneralLedgerSummary modifiedGeneralLedgerSummary)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        FIN_GeneralLedgerSummary original = ctx.FIN_GeneralLedgerSummary.Find(modifiedGeneralLedgerSummary.AccountNo, modifiedGeneralLedgerSummary.CurrencyCode, modifiedGeneralLedgerSummary.FinancialYear, modifiedGeneralLedgerSummary.AccountingPeriod);

                        if (original != null)
                        {
                            ctx.Entry(original).CurrentValues.SetValues(modifiedGeneralLedgerSummary);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("GeneralLedgerSummary with AccountNo:{0} CurrencyCode:{1} FinancialYear:{2} AccountingPeriod:{3}  was not found.", modifiedGeneralLedgerSummary.AccountNo, modifiedGeneralLedgerSummary.CurrencyCode, modifiedGeneralLedgerSummary.FinancialYear, modifiedGeneralLedgerSummary.AccountingPeriod)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateGeneralLedgerSummary : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Delete GeneralLedgerSummary record
        /// </summary>
        /// <param name="deletingGeneralLedgerSummary">The deleting general ledger summary.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteGeneralLedgerSummary(FIN_GeneralLedgerSummary deletingGeneralLedgerSummary)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        FIN_GeneralLedgerSummary original = ctx.FIN_GeneralLedgerSummary.Find(deletingGeneralLedgerSummary.AccountNo, deletingGeneralLedgerSummary.CurrencyCode, deletingGeneralLedgerSummary.FinancialYear, deletingGeneralLedgerSummary.AccountingPeriod);

                        if (original != null)
                        {
                            ctx.Entry(original).State = EntityState.Deleted;
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("GeneralLedgerSummary with AccountNo:{0} CurrencyCode:{1} FinancialYear:{2} AccountingPeriod:{3}  was not found.", deletingGeneralLedgerSummary.AccountNo, deletingGeneralLedgerSummary.CurrencyCode, deletingGeneralLedgerSummary.FinancialYear, deletingGeneralLedgerSummary.AccountingPeriod)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteGeneralLedgerSummary) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Generals the ledger summary exists.
        /// </summary>
        /// <param name="existsGeneralLedgerSummary">The exists general ledger summary.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool GeneralLedgerSummaryExists(FIN_GeneralLedgerSummary existsGeneralLedgerSummary)
        {
            try
            {

                using (var ctx = new MDMEntities())
                {
                    FIN_GeneralLedgerSummary original = ctx.FIN_GeneralLedgerSummary.Find(existsGeneralLedgerSummary.AccountNo, existsGeneralLedgerSummary.CurrencyCode, existsGeneralLedgerSummary.FinancialYear, existsGeneralLedgerSummary.AccountingPeriod);

                    if (original != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred GeneralLedgerSummary : {0} ", ex.Message));
                return false;
            }
        }
        #endregion

        #region PrimaryTransaction
        /// <summary>
        /// Queries the primary transaction.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>ResultDTO&lt;FIN_PrimaryTransaction&gt;.</returns>
        public ResultDTO<FIN_PrimaryTransaction> QueryPrimaryTransaction(PrimaryTransactionQuery query)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.FIN_PrimaryTransaction.AsEnumerable();

                    if (query.DocumentCode != null)
                    {
                        c = c.Where(i => i.DocCode == query.DocumentCode);
                    }
                    if (query.FinancialYear != null)
                    {
                        c = c.Where(i => i.FinancialYear == query.FinancialYear);
                    }
                    if (query.TransactionCode != null)
                    {
                        c = c.Where(i => i.TxCode == query.TransactionCode);
                    }
                    var result = new ResultDTO<FIN_PrimaryTransaction>() { Result = c.ToArray() };
                    return result;
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on QueryPrimaryTransaction : {0} ", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Creates the primary transaction.
        /// </summary>
        /// <param name="new_FIN_PrimaryTransaction">The new_ fi n_ primary transaction.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreatePrimaryTransaction(FIN_PrimaryTransaction new_FIN_PrimaryTransaction)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        ctx.FIN_PrimaryTransaction.Add(new_FIN_PrimaryTransaction);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (DbEntityValidationException dbEx)
            {
                String errorMsg = "Validation Errors were detected" + Environment.NewLine;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {

                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        errorMsg += String.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;
                    }

                }
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new PrimaryTransaction : {0} ", errorMsg));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = errorMsg };

            }
            catch (Exception ex)
            {

                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new PrimaryTransaction : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Reads the primary transactionby key.
        /// </summary>
        /// <param name="DocCode">The document code.</param>
        /// <param name="TxCode">The tx code.</param>
        /// <param name="TxSerial">The tx serial.</param>
        /// <param name="VoucherNumber">The voucher number.</param>
        /// <param name="FinancialYear">The financial year.</param>
        /// <returns>FIN_PrimaryTransaction.</returns>
        public FIN_PrimaryTransaction ReadPrimaryTransactionbyKey(string DocCode, string TxCode, int TxSerial, int VoucherNumber, int FinancialYear)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_PrimaryTransaction.Where(c => c.DocCode == DocCode && c.TxCode == TxCode && c.TxSerial == TxSerial && c.VoucherNumber == VoucherNumber && c.FinancialYear == FinancialYear).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadPrimaryTransactionbyKey : {0} ", ex.Message));
                return null;
            }
        }


        /// <summary>
        /// Return all PrimaryTransaction records
        /// </summary>
        /// <returns>FIN_PrimaryTransaction[].</returns>
        public FIN_PrimaryTransaction[] ReadAllPrimaryTransaction()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_PrimaryTransaction.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAllPrimaryTransaction : {0} ", ex.Message));
                return new FIN_PrimaryTransaction[0];
            }
        }

        /// <summary>
        /// Execute the query and Return PrimaryTransaction records
        /// </summary>
        /// <returns>IQueryable&lt;FIN_PrimaryTransaction&gt;.</returns>
        public IQueryable<FIN_PrimaryTransaction> ReadPrimaryTransaction()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_PrimaryTransaction.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadPrimaryTransaction : {0} ", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Update PrimaryTransaction record
        /// </summary>
        /// <param name="modifiedPrimaryTransaction">The modified primary transaction.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdatePrimaryTransaction(FIN_PrimaryTransaction modifiedPrimaryTransaction)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        FIN_PrimaryTransaction original = ctx.FIN_PrimaryTransaction.Find(modifiedPrimaryTransaction.DocCode, modifiedPrimaryTransaction.TxCode, modifiedPrimaryTransaction.TxSerial, modifiedPrimaryTransaction.VoucherNumber, modifiedPrimaryTransaction.FinancialYear);

                        if (original != null)
                        {
                            ctx.Entry(original).CurrentValues.SetValues(modifiedPrimaryTransaction);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("PrimaryTransaction with DocCode:{0} TxCode:{1} TxSerial:{2} VoucherNumber:{3} FinancialYear:{4}  was not found.", modifiedPrimaryTransaction.DocCode, modifiedPrimaryTransaction.TxCode, modifiedPrimaryTransaction.TxSerial, modifiedPrimaryTransaction.VoucherNumber, modifiedPrimaryTransaction.FinancialYear)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }

            }
            catch (DbEntityValidationException dbEx)
            {
                String errorMsg = "Validation Errors were detected" + Environment.NewLine;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {

                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        errorMsg += String.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;
                    }

                }
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new PrimaryTransaction : {0} ", errorMsg));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = errorMsg };

            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdatePrimaryTransaction : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Delete PrimaryTransaction record
        /// </summary>
        /// <param name="deletingPrimaryTransaction">The deleting primary transaction.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeletePrimaryTransaction(FIN_PrimaryTransaction deletingPrimaryTransaction)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        FIN_PrimaryTransaction original = ctx.FIN_PrimaryTransaction.Find(deletingPrimaryTransaction.DocCode, deletingPrimaryTransaction.TxCode, deletingPrimaryTransaction.TxSerial, deletingPrimaryTransaction.VoucherNumber, deletingPrimaryTransaction.FinancialYear);

                        if (original != null)
                        {
                            ctx.Entry(original).State = EntityState.Deleted;
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("PrimaryTransaction with DocCode:{0} TxCode:{1} TxSerial:{2} VoucherNumber:{3} FinancialYear:{4}  was not found.", deletingPrimaryTransaction.DocCode, deletingPrimaryTransaction.TxCode, deletingPrimaryTransaction.TxSerial, deletingPrimaryTransaction.VoucherNumber, deletingPrimaryTransaction.FinancialYear)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeletePrimaryTransaction) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Primaries the transaction exists.
        /// </summary>
        /// <param name="existsPrimaryTransaction">The exists primary transaction.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool PrimaryTransactionExists(FIN_PrimaryTransaction existsPrimaryTransaction)
        {
            try
            {

                using (var ctx = new MDMEntities())
                {
                    FIN_PrimaryTransaction original = ctx.FIN_PrimaryTransaction.Find(existsPrimaryTransaction.DocCode, existsPrimaryTransaction.TxCode, existsPrimaryTransaction.TxSerial, existsPrimaryTransaction.VoucherNumber, existsPrimaryTransaction.FinancialYear);

                    if (original != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred PrimaryTransaction : {0} ", ex.Message));
                return false;
            }
        }
        #endregion

        #region ProfitLostType


        /// <summary>
        /// Queries the type of the profit lost.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;FIN_ProfitLostType&gt;.</returns>
        public ResultDTO<FIN_ProfitLostType> QueryProfitLostType(ProfitLostQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.FIN_ProfitLostType.AsQueryable();

                    if (query.TypeId != null)
                    {
                        c = c.Where(i => i.TypeID == query.TypeId);
                    }

                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.TypeID).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<FIN_ProfitLostType>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on QueryPrimaryTransaction : {0} ", ex.Message));
                return null;
            }
        }


        /// <summary>
        /// Creates the type of the profit lost.
        /// </summary>
        /// <param name="new_FIN_ProfitLostType">Type of the new_ fi n_ profit lost.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateProfitLostType(FIN_ProfitLostType new_FIN_ProfitLostType)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        ctx.FIN_ProfitLostType.Add(new_FIN_ProfitLostType);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new ProfitLostType : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Reads the profit lost typeby key.
        /// </summary>
        /// <param name="TypeID">The type identifier.</param>
        /// <returns>FIN_ProfitLostType.</returns>
        public FIN_ProfitLostType ReadProfitLostTypebyKey(int TypeID)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_ProfitLostType.Where(c => c.TypeID == TypeID).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadProfitLostTypebyKey : {0} ", ex.Message));
                return null;
            }
        }


        /// <summary>
        /// Return all ProfitLostType records
        /// </summary>
        /// <returns>FIN_ProfitLostType[].</returns>
        public FIN_ProfitLostType[] ReadAllProfitLostType()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_ProfitLostType.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAllProfitLostType : {0} ", ex.Message));
                return new FIN_ProfitLostType[0];
            }
        }

        /// <summary>
        /// Execute the query and Return ProfitLostType records
        /// </summary>
        /// <returns>IQueryable&lt;FIN_ProfitLostType&gt;.</returns>
        public IQueryable<FIN_ProfitLostType> ReadProfitLostType()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_ProfitLostType.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadProfitLostType : {0} ", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Update ProfitLostType record
        /// </summary>
        /// <param name="modifiedProfitLostType">Type of the modified profit lost.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateProfitLostType(FIN_ProfitLostType modifiedProfitLostType)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        FIN_ProfitLostType original = ctx.FIN_ProfitLostType.Find(modifiedProfitLostType.TypeID);

                        if (original != null)
                        {
                            ctx.Entry(original).CurrentValues.SetValues(modifiedProfitLostType);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("ProfitLostType with TypeID:{0}  was not found.", modifiedProfitLostType.TypeID)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateProfitLostType : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Delete ProfitLostType record
        /// </summary>
        /// <param name="deletingProfitLostType">Type of the deleting profit lost.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteProfitLostType(FIN_ProfitLostType deletingProfitLostType)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        FIN_ProfitLostType original = ctx.FIN_ProfitLostType.Find(deletingProfitLostType.TypeID);

                        if (original != null)
                        {
                            ctx.Entry(original).State = EntityState.Deleted;
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("ProfitLostType with TypeID:{0}  was not found.", deletingProfitLostType.TypeID)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteProfitLostType) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Profits the lost type exists.
        /// </summary>
        /// <param name="existsProfitLostType">Type of the exists profit lost.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool ProfitLostTypeExists(FIN_ProfitLostType existsProfitLostType)
        {
            try
            {

                using (var ctx = new MDMEntities())
                {
                    FIN_ProfitLostType original = ctx.FIN_ProfitLostType.Find(existsProfitLostType.TypeID);

                    if (original != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred ProfitLostType : {0} ", ex.Message));
                return false;
            }
        }
        #endregion

        #region SecondaryTransaction

        /// <summary>
        /// Creates the secondary transaction.
        /// </summary>
        /// <param name="new_FIN_SecondaryTransaction">The new_ fi n_ secondary transaction.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateSecondaryTransaction(FIN_SecondaryTransaction new_FIN_SecondaryTransaction)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        ctx.FIN_SecondaryTransaction.Add(new_FIN_SecondaryTransaction);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new SecondaryTransaction : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Reads the secondary transactionby key.
        /// </summary>
        /// <param name="DocCode">The document code.</param>
        /// <param name="TxCode">The tx code.</param>
        /// <param name="TxSerial">The tx serial.</param>
        /// <param name="VoucherNumber">The voucher number.</param>
        /// <param name="FinancialYear">The financial year.</param>
        /// <param name="TxnSeqNo">The TXN seq no.</param>
        /// <returns>FIN_SecondaryTransaction.</returns>
        public FIN_SecondaryTransaction ReadSecondaryTransactionbyKey(string DocCode, string TxCode, int TxSerial, int VoucherNumber, int FinancialYear, int TxnSeqNo)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_SecondaryTransaction.Where(c => c.DocCode == DocCode && c.TxCode == TxCode && c.TxSerial == TxSerial && c.VoucherNumber == VoucherNumber && c.FinancialYear == FinancialYear && c.TxnSeqNo == TxnSeqNo).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadSecondaryTransactionbyKey : {0} ", ex.Message));
                return null;
            }
        }


        /// <summary>
        /// Return all SecondaryTransaction records
        /// </summary>
        /// <returns>FIN_SecondaryTransaction[].</returns>
        public FIN_SecondaryTransaction[] ReadAllSecondaryTransaction()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_SecondaryTransaction.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAllSecondaryTransaction : {0} ", ex.Message));
                return new FIN_SecondaryTransaction[0];
            }
        }

        /// <summary>
        /// Execute the query and Return SecondaryTransaction records
        /// </summary>
        /// <returns>IQueryable&lt;FIN_SecondaryTransaction&gt;.</returns>
        public IQueryable<FIN_SecondaryTransaction> ReadSecondaryTransaction()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_SecondaryTransaction.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadSecondaryTransaction : {0} ", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Update SecondaryTransaction record
        /// </summary>
        /// <param name="modifiedSecondaryTransaction">The modified secondary transaction.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateSecondaryTransaction(FIN_SecondaryTransaction modifiedSecondaryTransaction)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        FIN_SecondaryTransaction original = ctx.FIN_SecondaryTransaction.Find(modifiedSecondaryTransaction.DocCode, modifiedSecondaryTransaction.TxCode, modifiedSecondaryTransaction.TxSerial, modifiedSecondaryTransaction.VoucherNumber, modifiedSecondaryTransaction.FinancialYear, modifiedSecondaryTransaction.TxnSeqNo);

                        if (original != null)
                        {
                            ctx.Entry(original).CurrentValues.SetValues(modifiedSecondaryTransaction);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("SecondaryTransaction with DocCode:{0} TxCode:{1} TxSerial:{2} VoucherNumber:{3} FinancialYear:{4} TxnSeqNo:{5}  was not found.", modifiedSecondaryTransaction.DocCode, modifiedSecondaryTransaction.TxCode, modifiedSecondaryTransaction.TxSerial, modifiedSecondaryTransaction.VoucherNumber, modifiedSecondaryTransaction.FinancialYear, modifiedSecondaryTransaction.TxnSeqNo)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateSecondaryTransaction : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Delete SecondaryTransaction record
        /// </summary>
        /// <param name="deletingSecondaryTransaction">The deleting secondary transaction.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteSecondaryTransaction(FIN_SecondaryTransaction deletingSecondaryTransaction)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        FIN_SecondaryTransaction original = ctx.FIN_SecondaryTransaction.Find(deletingSecondaryTransaction.DocCode, deletingSecondaryTransaction.TxCode, deletingSecondaryTransaction.TxSerial, deletingSecondaryTransaction.VoucherNumber, deletingSecondaryTransaction.FinancialYear, deletingSecondaryTransaction.TxnSeqNo);

                        if (original != null)
                        {
                            ctx.Entry(original).State = EntityState.Deleted;
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("SecondaryTransaction with DocCode:{0} TxCode:{1} TxSerial:{2} VoucherNumber:{3} FinancialYear:{4} TxnSeqNo:{5}  was not found.", deletingSecondaryTransaction.DocCode, deletingSecondaryTransaction.TxCode, deletingSecondaryTransaction.TxSerial, deletingSecondaryTransaction.VoucherNumber, deletingSecondaryTransaction.FinancialYear, deletingSecondaryTransaction.TxnSeqNo)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteSecondaryTransaction) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Secondaries the transaction exists.
        /// </summary>
        /// <param name="existsSecondaryTransaction">The exists secondary transaction.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool SecondaryTransactionExists(FIN_SecondaryTransaction existsSecondaryTransaction)
        {
            try
            {

                using (var ctx = new MDMEntities())
                {
                    FIN_SecondaryTransaction original = ctx.FIN_SecondaryTransaction.Find(existsSecondaryTransaction.DocCode, existsSecondaryTransaction.TxCode, existsSecondaryTransaction.TxSerial, existsSecondaryTransaction.VoucherNumber, existsSecondaryTransaction.FinancialYear, existsSecondaryTransaction.TxnSeqNo);

                    if (original != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred SecondaryTransaction : {0} ", ex.Message));
                return false;
            }
        }
        #endregion

        #region SpecialAccountType

        /// <summary>
        /// Queries the type of the special account.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;FIN_SpecialAccountType&gt;.</returns>
        public ResultDTO<FIN_SpecialAccountType> QuerySpecialAccountType(SpecialAccountQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.FIN_SpecialAccountType.AsQueryable();

                    if (query.TypeId != null)
                    {
                        c = c.Where(i => i.TypeID == query.TypeId);
                    }

                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.TypeID).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<FIN_SpecialAccountType>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on QuerySpecialAccountType : {0} ", ex.Message));
                return null;
            }
        }



        /// <summary>
        /// Creates the type of the special account.
        /// </summary>
        /// <param name="new_FIN_SpecialAccountType">Type of the new_ fi n_ special account.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateSpecialAccountType(FIN_SpecialAccountType new_FIN_SpecialAccountType)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        ctx.FIN_SpecialAccountType.Add(new_FIN_SpecialAccountType);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new SpecialAccountType : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Reads the special account typeby key.
        /// </summary>
        /// <param name="TypeID">The type identifier.</param>
        /// <returns>FIN_SpecialAccountType.</returns>
        public FIN_SpecialAccountType ReadSpecialAccountTypebyKey(int TypeID)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_SpecialAccountType.Where(c => c.TypeID == TypeID).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadSpecialAccountTypebyKey : {0} ", ex.Message));
                return null;
            }
        }


        /// <summary>
        /// Return all SpecialAccountType records
        /// </summary>
        /// <returns>FIN_SpecialAccountType[].</returns>
        public FIN_SpecialAccountType[] ReadAllSpecialAccountType()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_SpecialAccountType.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAllSpecialAccountType : {0} ", ex.Message));
                return new FIN_SpecialAccountType[0];
            }
        }

        /// <summary>
        /// Execute the query and Return SpecialAccountType records
        /// </summary>
        /// <returns>IQueryable&lt;FIN_SpecialAccountType&gt;.</returns>
        public IQueryable<FIN_SpecialAccountType> ReadSpecialAccountType()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_SpecialAccountType.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadSpecialAccountType : {0} ", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Update SpecialAccountType record
        /// </summary>
        /// <param name="modifiedSpecialAccountType">Type of the modified special account.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateSpecialAccountType(FIN_SpecialAccountType modifiedSpecialAccountType)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        FIN_SpecialAccountType original = ctx.FIN_SpecialAccountType.Find(modifiedSpecialAccountType.TypeID);

                        if (original != null)
                        {
                            ctx.Entry(original).CurrentValues.SetValues(modifiedSpecialAccountType);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("SpecialAccountType with TypeID:{0}  was not found.", modifiedSpecialAccountType.TypeID)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateSpecialAccountType : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Delete SpecialAccountType record
        /// </summary>
        /// <param name="deletingSpecialAccountType">Type of the deleting special account.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteSpecialAccountType(FIN_SpecialAccountType deletingSpecialAccountType)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        FIN_SpecialAccountType original = ctx.FIN_SpecialAccountType.Find(deletingSpecialAccountType.TypeID);

                        if (original != null)
                        {
                            ctx.Entry(original).State = EntityState.Deleted;
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("SpecialAccountType with TypeID:{0}  was not found.", deletingSpecialAccountType.TypeID)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteSpecialAccountType) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Specials the account type exists.
        /// </summary>
        /// <param name="existsSpecialAccountType">Type of the exists special account.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool SpecialAccountTypeExists(FIN_SpecialAccountType existsSpecialAccountType)
        {
            try
            {

                using (var ctx = new MDMEntities())
                {
                    FIN_SpecialAccountType original = ctx.FIN_SpecialAccountType.Find(existsSpecialAccountType.TypeID);

                    if (original != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred SpecialAccountType : {0} ", ex.Message));
                return false;
            }
        }
        #endregion

        #region TxnReference

        /// <summary>
        /// Creates the TXN reference.
        /// </summary>
        /// <param name="new_FIN_TxnReference">The new_ fi n_ TXN reference.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateTxnReference(FIN_TxnReference new_FIN_TxnReference)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        var docAttributes = ctx.ERP_DocumentAttributes.Find(new_FIN_TxnReference.ERP_DocumentAttributes.DocCode, new_FIN_TxnReference.ERP_DocumentAttributes.TxCode);
                        new_FIN_TxnReference.ERP_DocumentAttributes = docAttributes;
                        ctx.FIN_TxnReference.Add(new_FIN_TxnReference);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new TxnReference : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Queries the TXN reference.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;FIN_TxnReference&gt;.</returns>
        public ResultDTO<FIN_TxnReference> QueryTxnReference(TransactionReferenceQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.FIN_TxnReference.AsQueryable();

                    if (query.DocCode != null)
                    {
                        c = c.Where(i => i.DocCode == query.DocCode);
                    }
                    if (query.RefSeq != null)
                    {
                        c = c.Where(i => i.RefSeq == query.RefSeq);
                    }
                    if (query.ReferenceText != null)
                    {
                        c = c.Where(i => i.ReferenceText.Contains(query.ReferenceText));
                    }
                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.DocCode).Skip((pageNumber - 1) * pageSize).Take(pageSize).Include(s => s.ERP_DocumentAttributes).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<FIN_TxnReference>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on Query : {0} ", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Reads the TXN referenceby key.
        /// </summary>
        /// <param name="DocCode">The document code.</param>
        /// <param name="TxCode">The tx code.</param>
        /// <param name="RefSeq">The reference seq.</param>
        /// <returns>FIN_TxnReference.</returns>
        public FIN_TxnReference ReadTxnReferencebyKey(string DocCode, string TxCode, short RefSeq)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_TxnReference.Where(c => c.DocCode == DocCode && c.TxCode == TxCode && c.RefSeq == RefSeq).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadTxnReferencebyKey : {0} ", ex.Message));
                return null;
            }
        }


        /// <summary>
        /// Return all TxnReference records
        /// </summary>
        /// <returns>FIN_TxnReference[].</returns>
        public FIN_TxnReference[] ReadAllTxnReference()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_TxnReference.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAllTxnReference : {0} ", ex.Message));
                return new FIN_TxnReference[0];
            }
        }

        /// <summary>
        /// Execute the query and Return TxnReference records
        /// </summary>
        /// <returns>IQueryable&lt;FIN_TxnReference&gt;.</returns>
        public IQueryable<FIN_TxnReference> ReadTxnReference()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_TxnReference.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadTxnReference : {0} ", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Update TxnReference record
        /// </summary>
        /// <param name="modifiedTxnReference">The modified TXN reference.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateTxnReference(FIN_TxnReference modifiedTxnReference)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {

                        FIN_TxnReference original = ctx.FIN_TxnReference.Find(modifiedTxnReference.ERP_DocumentAttributes.DocCode, modifiedTxnReference.ERP_DocumentAttributes.TxCode, modifiedTxnReference.RefSeq);

                        if (original != null)
                        {
                            ctx.Entry(original).CurrentValues.SetValues(modifiedTxnReference);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("TxnReference with DocCode:{0} TxCode:{1} RefSeq:{2}  was not found.", modifiedTxnReference.DocCode, modifiedTxnReference.TxCode, modifiedTxnReference.RefSeq)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateTxnReference : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Delete TxnReference record
        /// </summary>
        /// <param name="deletingTxnReference">The deleting TXN reference.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteTxnReference(FIN_TxnReference deletingTxnReference)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        FIN_TxnReference original = ctx.FIN_TxnReference.Find(deletingTxnReference.ERP_DocumentAttributes.DocCode, deletingTxnReference.ERP_DocumentAttributes.TxCode, deletingTxnReference.RefSeq);

                        if (original != null)
                        {
                            ctx.Entry(original).State = EntityState.Deleted;
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("TxnReference with DocCode:{0} TxCode:{1} RefSeq:{2}  was not found.", deletingTxnReference.DocCode, deletingTxnReference.TxCode, deletingTxnReference.RefSeq)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteTxnReference) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// TXNs the reference exists.
        /// </summary>
        /// <param name="existsTxnReference">The exists TXN reference.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool TxnReferenceExists(FIN_TxnReference existsTxnReference)
        {
            try
            {

                using (var ctx = new MDMEntities())
                {
                    FIN_TxnReference original = ctx.FIN_TxnReference.Find(existsTxnReference.ERP_DocumentAttributes.DocCode, existsTxnReference.ERP_DocumentAttributes.TxCode, existsTxnReference.RefSeq);

                    if (original != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred TxnReference : {0} ", ex.Message));
                return false;
            }
        }
        #endregion

        #region GeneralLedgerAccount

        /// <summary>
        /// Creates the general ledger account.
        /// </summary>
        /// <param name="new_FIN_GeneralLedgerAccount">The new_ fi n_ general ledger account.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateGeneralLedgerAccount(FIN_GeneralLedgerAccount new_FIN_GeneralLedgerAccount)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        ctx.FIN_GeneralLedgerAccount.Add(new_FIN_GeneralLedgerAccount);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new GeneralLedgerAccount : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Reads the general ledger accountby key.
        /// </summary>
        /// <param name="AccountNo">The account no.</param>
        /// <returns>FIN_GeneralLedgerAccount.</returns>
        public FIN_GeneralLedgerAccount ReadGeneralLedgerAccountbyKey(string AccountNo)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_GeneralLedgerAccount.Where(c => c.AccountNo == AccountNo).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadGeneralLedgerAccountbyKey : {0} ", ex.Message));
                return null;
            }
        }


        /// <summary>
        /// Return all GeneralLedgerAccount records
        /// </summary>
        /// <returns>FIN_GeneralLedgerAccount[].</returns>
        public FIN_GeneralLedgerAccount[] ReadAllGeneralLedgerAccount()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_GeneralLedgerAccount.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAllGeneralLedgerAccount : {0} ", ex.Message));
                return new FIN_GeneralLedgerAccount[0];
            }
        }

        /// <summary>
        /// Execute the query and Return GeneralLedgerAccount records
        /// </summary>
        /// <returns>IQueryable&lt;FIN_GeneralLedgerAccount&gt;.</returns>
        public IQueryable<FIN_GeneralLedgerAccount> ReadGeneralLedgerAccount()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_GeneralLedgerAccount.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadGeneralLedgerAccount : {0} ", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Update GeneralLedgerAccount record
        /// </summary>
        /// <param name="modifiedGeneralLedgerAccount">The modified general ledger account.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateGeneralLedgerAccount(FIN_GeneralLedgerAccount modifiedGeneralLedgerAccount)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        FIN_GeneralLedgerAccount original = ctx.FIN_GeneralLedgerAccount.Find(modifiedGeneralLedgerAccount.AccountNo);

                        if (original != null)
                        {
                            ctx.Entry(original).CurrentValues.SetValues(modifiedGeneralLedgerAccount);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("GeneralLedgerAccount with AccountNo:{0}  was not found.", modifiedGeneralLedgerAccount.AccountNo)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateGeneralLedgerAccount : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Delete GeneralLedgerAccount record
        /// </summary>
        /// <param name="deletingGeneralLedgerAccount">The deleting general ledger account.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteGeneralLedgerAccount(FIN_GeneralLedgerAccount deletingGeneralLedgerAccount)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        FIN_GeneralLedgerAccount original = ctx.FIN_GeneralLedgerAccount.Find(deletingGeneralLedgerAccount.AccountNo);

                        if (original != null)
                        {
                            ctx.Entry(original).State = EntityState.Deleted;
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("GeneralLedgerAccount with AccountNo:{0}  was not found.", deletingGeneralLedgerAccount.AccountNo)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteGeneralLedgerAccount) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Generals the ledger account exists.
        /// </summary>
        /// <param name="existsGeneralLedgerAccount">The exists general ledger account.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool GeneralLedgerAccountExists(FIN_GeneralLedgerAccount existsGeneralLedgerAccount)
        {
            try
            {

                using (var ctx = new MDMEntities())
                {
                    FIN_GeneralLedgerAccount original = ctx.FIN_GeneralLedgerAccount.Find(existsGeneralLedgerAccount.AccountNo);

                    if (original != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred GeneralLedgerAccount : {0} ", ex.Message));
                return false;
            }
        }

        /// <summary>
        /// Queries the general ledger account.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;FIN_GeneralLedgerAccount&gt;.</returns>
        public ResultDTO<FIN_GeneralLedgerAccount> QueryGeneralLedgerAccount(GeneralLedgerAccountQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    var c = ctx.FIN_GeneralLedgerAccount.AsQueryable();

                    if (query.AccountNo != null)
                    {
                        c = c.Where(i => i.AccountNo == query.AccountNo);
                    }
                    if (query.AccountType != null)
                    {
                        c = c.Where(i => i.AccountType == query.AccountType);
                    }
                    if (query.AccountSubtype != null)
                    {
                        c = c.Where(i => i.AccountSubtype == query.AccountSubtype);
                    }
                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.AccountNo).Skip((pageNumber - 1) * pageSize).Take(pageSize).Include(s => s.FIN_ProfitLostType).Include(s => s.FIN_SpecialAccountType).Include(s => s.FIN_AccountSubTypeCategory).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<FIN_GeneralLedgerAccount>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on QueryUnitConversion : {0} ", ex.Message));
                return null;
            }
        }

        #endregion

        #region Currency

        /// <summary>
        /// Create a new Currency record
        /// </summary>
        /// <param name="new_Currency">The new_ currency.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateCurrency(FIN_Currency new_Currency)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        ctx.FIN_Currency.Add(new_Currency);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new Currency : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Read a Currency record
        /// </summary>
        /// <param name="CurrencyCode">The currency code.</param>
        /// <returns>FIN_Currency.</returns>
        public FIN_Currency ReadCurrencyByKey(string CurrencyCode)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_Currency.Where(c => c.CurrencyCode == CurrencyCode).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on read Currency by key : {0} ", ex.Message));
                return null;
            }
        }


        /// <summary>
        /// Return all Currency records
        /// </summary>
        /// <returns>FIN_Currency[].</returns>
        public FIN_Currency[] ReadAllCurrency()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_Currency.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on read all Currency : {0} ", ex.Message));
                return new FIN_Currency[0];
            }
        }


        /// <summary>
        /// Update Currency record
        /// </summary>
        /// <param name="modified_Currency">The modified_ currency.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateCurrency(FIN_Currency modified_Currency)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        FIN_Currency original_Currency = ctx.FIN_Currency.Find(modified_Currency.CurrencyCode);

                        if (original_Currency != null)
                        {
                            ctx.Entry(original_Currency).CurrentValues.SetValues(modified_Currency);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage = string.Format("Currency with CurrencyCode:{0}  was not found.", modified_Currency.CurrencyCode)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on update Currency : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Delete Currency record
        /// </summary>
        /// <param name="delete_Currency">The delete_ currency.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteCurrency(FIN_Currency delete_Currency)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        FIN_Currency original_Currency = ctx.FIN_Currency.Find(delete_Currency.CurrencyCode);

                        if (original_Currency != null)
                        {
                            ctx.FIN_Currency.Remove(original_Currency);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage = string.Format("Currency with CurrencyCode:{0}  was not found.", delete_Currency.CurrencyCode)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on delete Currency) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Exists the currency.
        /// </summary>
        /// <param name="exist_Currency">The exist_ currency.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool ExistCurrency(FIN_Currency exist_Currency)
        {
            try
            {

                using (var ctx = new MDMEntities())
                {
                    FIN_Currency original_Currency = ctx.FIN_Currency.Find(exist_Currency.CurrencyCode);

                    if (original_Currency != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred Currency : {0} ", ex.Message));
                return false;
            }
        }

        /// <summary>
        /// Execute the query and Return Currency records
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;FIN_Currency&gt;.</returns>
        public ResultDTO<FIN_Currency> QueryCurrency(CurrencyQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    //ctx.Configuration.ProxyCreationEnabled = false;
                    var c = ctx.FIN_Currency.AsQueryable();
                    if (query.CurrencyCode != null)
                    {
                        c = c.Where(i => i.CurrencyCode == query.CurrencyCode);
                    }
                    if (query.CurrencyName != null)
                    {
                        c = c.Where(i => i.CurrencyName.Contains(query.CurrencyName));
                    }
                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.CurrencyCode).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<FIN_Currency>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on QueryUnitConversion : {0} ", ex.Message));
                return null;
            }
        }

        #endregion

        #region CurrencyExchangeRate

        /// <summary>
        /// Create a new Currency Exchange Rate record
        /// </summary>
        /// <param name="new_CurrencyExchangeRate">The new_ currency exchange rate.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateCurrencyExchangeRate(FIN_CurrencyExchangeRate new_CurrencyExchangeRate)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        var currency_code = ctx.FIN_Currency.Find(new_CurrencyExchangeRate.FIN_Currency.CurrencyCode);
                        new_CurrencyExchangeRate.FIN_Currency = currency_code;
                        ctx.FIN_CurrencyExchangeRate.Add(new_CurrencyExchangeRate);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new Currency Exchange Rate : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Return a Currency Exchange Rate record
        /// </summary>
        /// <param name="CurrencyCode">The currency code.</param>
        /// <param name="CalenderYear">The calender year.</param>
        /// <param name="CalenderMonth">The calender month.</param>
        /// <returns>FIN_CurrencyExchangeRate.</returns>
        public FIN_CurrencyExchangeRate ReadCurrencyExchangeRateByKey(string CurrencyCode, short CalenderYear, byte CalenderMonth)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_CurrencyExchangeRate.Where(c => c.CurrencyCode == CurrencyCode && c.CalenderYear == CalenderYear && c.CalenderMonth == CalenderMonth).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on read Currency Exchange Rate by key : {0} ", ex.Message));
                return null;
            }
        }


        /// <summary>
        /// Return all CurrencyExchangeRate records
        /// </summary>
        /// <returns>FIN_CurrencyExchangeRate[].</returns>
        public FIN_CurrencyExchangeRate[] ReadAllCurrencyExchangeRate()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.FIN_CurrencyExchangeRate.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on read all Currency Exchange Rate : {0} ", ex.Message));
                return new FIN_CurrencyExchangeRate[0];
            }
        }


        /// <summary>
        /// Update Currency Exchange Rate record
        /// </summary>
        /// <param name="modified_CurrencyExchangeRate">The modified_ currency exchange rate.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateCurrencyExchangeRate(FIN_CurrencyExchangeRate modified_CurrencyExchangeRate)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        FIN_CurrencyExchangeRate original_Currency = ctx.FIN_CurrencyExchangeRate.Find(modified_CurrencyExchangeRate.CurrencyCode, modified_CurrencyExchangeRate.CalenderYear, modified_CurrencyExchangeRate.CalenderMonth);

                        if (original_Currency != null)
                        {
                            ctx.Entry(original_Currency).CurrentValues.SetValues(modified_CurrencyExchangeRate);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage = string.Format("Currency Exchange Rate with CurrencyCode:{0} CalenderYear:{1} CalenderMonth:{2}  was not found.", modified_CurrencyExchangeRate.CurrencyCode, modified_CurrencyExchangeRate.CalenderYear, modified_CurrencyExchangeRate.CalenderMonth)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on update Currency Exchange Rate : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Delete Currency Exchange Rate record
        /// </summary>
        /// <param name="delete_CurrencyExchangeRate">The delete_ currency exchange rate.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteCurrencyExchangeRate(FIN_CurrencyExchangeRate delete_CurrencyExchangeRate)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        FIN_CurrencyExchangeRate original_Currency = ctx.FIN_CurrencyExchangeRate.Find(delete_CurrencyExchangeRate.CurrencyCode, delete_CurrencyExchangeRate.CalenderYear, delete_CurrencyExchangeRate.CalenderMonth);

                        if (original_Currency != null)
                        {
                            ctx.FIN_CurrencyExchangeRate.Remove(original_Currency);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("Currency Exchange Rate with CurrencyCode:{0} CalenderYear:{1} CalenderMonth:{2}  was not found.", delete_CurrencyExchangeRate.CurrencyCode, delete_CurrencyExchangeRate.CalenderYear, delete_CurrencyExchangeRate.CalenderMonth)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on delete Currency Exchange Rate) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Exists the currency exchange rate.
        /// </summary>
        /// <param name="exist_CurrencyExchangeRate">The exist_ currency exchange rate.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool ExistCurrencyExchangeRate(FIN_CurrencyExchangeRate exist_CurrencyExchangeRate)
        {
            try
            {

                using (var ctx = new MDMEntities())
                {
                    FIN_CurrencyExchangeRate original_Currency = ctx.FIN_CurrencyExchangeRate.Find(exist_CurrencyExchangeRate.FIN_Currency.CurrencyCode, exist_CurrencyExchangeRate.CalenderYear, exist_CurrencyExchangeRate.CalenderMonth);

                    if (original_Currency != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred Currency Exchange Rate : {0} ", ex.Message));
                return false;
            }
        }
        /// <summary>
        /// Execute the query and Return CurrencyExchangeRate records
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;FIN_CurrencyExchangeRate&gt;.</returns>

        public ResultDTO<FIN_CurrencyExchangeRate> QueryCurrencyExchangeRate(CurrencyExchangeRateQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    //ctx.Configuration.ProxyCreationEnabled = false;
                    var c = ctx.FIN_CurrencyExchangeRate.AsQueryable();
                    if (query.CurrencyCode != null)
                    {
                        c = c.Where(i => i.CurrencyCode == query.CurrencyCode);

                    }
                    if (query.CalenderYear != null)
                    {
                        c = c.Where(i => i.CalenderYear == query.CalenderYear);

                    }
                    if (query.CalenderMonth != null)
                    {
                        c = c.Where(i => i.CalenderMonth == query.CalenderMonth);

                    }
                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.CurrencyCode).Skip((pageNumber - 1) * pageSize).Include(s => s.FIN_Currency).Take(pageSize).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<FIN_CurrencyExchangeRate>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on QueryUnitConversion : {0} ", ex.Message));
                return null;
            }
        }
        #endregion

        #region Period

        /// <summary>
        /// Execute the query and return Period records
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;ERP_Period&gt;.</returns>
        public ResultDTO<ERP_Period> QueryPeriod(PeriodQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.ERP_Period.AsQueryable();

                    if (query.ProductCode != null)
                    {
                        c = c.Where(i => i.ProductCode == query.ProductCode);
                    }
                    if (query.FinancialYear != null)
                    {
                        c = c.Where(i => i.FinancialYear == query.FinancialYear);
                    }
                    if (query.AccountingPeriod != null)
                    {
                        c = c.Where(i => i.AccountingPeriod == query.AccountingPeriod);
                    }

                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.ProductCode).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<ERP_Period>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on QueryProcessControl : {0} ", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Create a new Period record
        /// </summary>
        /// <param name="new_Period">The new_ period.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreatePeriod(ERP_Period new_Period)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        ctx.ERP_Period.Add(new_Period);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new Period : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Return a Period record
        /// </summary>
        /// <param name="ProductCode">The product code.</param>
        /// <param name="FinancialYear">The financial year.</param>
        /// <param name="AccountingPeriod">The accounting period.</param>
        /// <returns>ERP_Period.</returns>
        public ERP_Period ReadPeriodByKey(string ProductCode, int FinancialYear, int AccountingPeriod)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.ERP_Period.Where(c => c.ProductCode == ProductCode && c.FinancialYear == FinancialYear && c.AccountingPeriod == AccountingPeriod).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on read Period by Key : {0} ", ex.Message));
                return null;
            }
        }


        /// <summary>
        /// Return all Period records
        /// </summary>
        /// <returns>ERP_Period[].</returns>
        public ERP_Period[] ReadAllPeriod()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.ERP_Period.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on read all Period : {0} ", ex.Message));
                return new ERP_Period[0];
            }
        }

        /// <summary>
        /// Update Period record
        /// </summary>
        /// <param name="modified_Period">The modified_ period.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdatePeriod(ERP_Period modified_Period)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        ERP_Period original_Period = ctx.ERP_Period.Find(modified_Period.ProductCode, modified_Period.FinancialYear, modified_Period.AccountingPeriod);

                        if (original_Period != null)
                        {
                            ctx.Entry(original_Period).CurrentValues.SetValues(modified_Period);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage = string.Format("Period with ProductCode:{0} FinancialYear:{1} AccountingPeriod:{2}  was not found.", modified_Period.ProductCode, modified_Period.FinancialYear, modified_Period.AccountingPeriod)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on update Period : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Delete Period record
        /// </summary>
        /// <param name="delete_Period">The delete_ period.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeletePeriod(ERP_Period delete_Period)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        ERP_Period original_Period = ctx.ERP_Period.Find(delete_Period.ProductCode, delete_Period.FinancialYear, delete_Period.AccountingPeriod);

                        if (original_Period != null)
                        {
                            ctx.ERP_Period.Remove(original_Period);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage = string.Format("Period with ProductCode:{0} FinancialYear:{1} AccountingPeriod:{2}  was not found.", delete_Period.ProductCode, delete_Period.FinancialYear, delete_Period.AccountingPeriod)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on delete Period) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Exists the period.
        /// </summary>
        /// <param name="exist_Period">The exist_ period.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool ExistPeriod(ERP_Period exist_Period)
        {
            try
            {

                using (var ctx = new MDMEntities())
                {
                    ERP_Period original_Period = ctx.ERP_Period.Find(exist_Period.ProductCode, exist_Period.FinancialYear, exist_Period.AccountingPeriod);

                    if (original_Period != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred Period : {0} ", ex.Message));
                return false;
            }
        }


        #endregion

        #region ProcessControl

        /// <summary>
        /// Execute the query and return Process Control records
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;ERP_ProcessControl&gt;.</returns>

        public ResultDTO<ERP_ProcessControl> QueryProcessControl(ProcessControlQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.ERP_ProcessControl.AsQueryable();

                    if (query.ProductCode != null)
                    {
                        c = c.Where(i => i.ProductCode == query.ProductCode);
                    }

                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.ProductCode).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<ERP_ProcessControl>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on Query Process Control : {0} ", ex.Message));
                return null;
            }
        }


        /// <summary>
        /// Create a new Process Control record
        /// </summary>
        /// <param name="new_ProcessControl">The new_ process control.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateProcessControl(ERP_ProcessControl new_ProcessControl)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        ctx.ERP_ProcessControl.Add(new_ProcessControl);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new Process Control : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Return a Process Control record
        /// </summary>
        /// <param name="ProductCode">The product code.</param>
        /// <returns>ERP_ProcessControl.</returns>
        public ERP_ProcessControl ReadProcessControlByKey(string ProductCode)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.ERP_ProcessControl.Where(c => c.ProductCode == ProductCode).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on read Process Control by Key : {0} ", ex.Message));
                return null;
            }
        }


        /// <summary>
        /// Return all ProcessControl records
        /// </summary>
        /// <returns>ERP_ProcessControl[].</returns>
        public ERP_ProcessControl[] ReadAllProcessControl()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.ERP_ProcessControl.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on read all Process Control : {0} ", ex.Message));
                return new ERP_ProcessControl[0];
            }
        }

        /// <summary>
        /// Update ProcessControl record
        /// </summary>
        /// <param name="modified_ProcessControl">The modified_ process control.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateProcessControl(ERP_ProcessControl modified_ProcessControl)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        ERP_ProcessControl original_ProcessControl = ctx.ERP_ProcessControl.Find(modified_ProcessControl.ProductCode);

                        if (original_ProcessControl != null)
                        {
                            ctx.Entry(original_ProcessControl).CurrentValues.SetValues(modified_ProcessControl);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage = string.Format("Process Control with ProductCode:{0}  was not found.", modified_ProcessControl.ProductCode)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on update Process Control : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Delete ProcessControl record
        /// </summary>
        /// <param name="delete_ProcessControl">The delete_ process control.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteProcessControl(ERP_ProcessControl delete_ProcessControl)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        ERP_ProcessControl original_ProcessControl = ctx.ERP_ProcessControl.Find(delete_ProcessControl.ProductCode);

                        if (original_ProcessControl != null)
                        {
                            ctx.ERP_ProcessControl.Remove(original_ProcessControl);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage = string.Format("Process Control with ProductCode:{0}  was not found.", delete_ProcessControl.ProductCode)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on delete Process Control) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Exists the process control.
        /// </summary>
        /// <param name="exist_ProcessControl">The exist_ process control.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool ExistProcessControl(ERP_ProcessControl exist_ProcessControl)
        {
            try
            {

                using (var ctx = new MDMEntities())
                {
                    ERP_ProcessControl original_ProcessControl = ctx.ERP_ProcessControl.Find(exist_ProcessControl.ProductCode);

                    if (original_ProcessControl != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred Process Control : {0} ", ex.Message));
                return false;
            }
        }

        #endregion

    }
}
