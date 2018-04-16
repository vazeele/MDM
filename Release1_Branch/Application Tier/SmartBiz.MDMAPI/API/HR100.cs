/// <summary>
/// HR Master Data API : This API is used to communicate with the HR Master Entities
/// </summary>
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

    public class HR100 : IDisposable
    {
        public void Dispose()
        {
            GC.Collect();
        }

        #region Employee
        /// <summary>
        /// Creates the employee.
        /// </summary>
        /// <param name="new_HR_EMPLOYEE">The new_ h r_ employee.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateEmployee(HR_EMPLOYEE new_HR_EMPLOYEE)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        ctx.HR_EMPLOYEE.Add(new_HR_EMPLOYEE);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new Employee : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Reads the employeeby key.
        /// </summary>
        /// <param name="EMP_NUMBER">The em p_ number.</param>
        /// <returns>HR_EMPLOYEE.</returns>
        public HR_EMPLOYEE ReadEmployeebyKey(string EMP_NUMBER)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMPLOYEE.Where(c => c.EMP_NUMBER == EMP_NUMBER).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadEmployeebyKey : {0} ", ex.Message));
                return null;
            }
        }
        /// <summary>
        /// Return all Employee records
        /// </summary>
        /// <returns>HR_EMPLOYEE[].</returns>
        public HR_EMPLOYEE[] ReadAllEmployee()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMPLOYEE.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAllEmployee : {0} ", ex.Message));
                return new HR_EMPLOYEE[0];
            }
        }
        /// <summary>
        /// Execute the query and Return Employee records
        /// </summary>
        /// <returns>IQueryable&lt;HR_EMPLOYEE&gt;.</returns>
        public IQueryable<HR_EMPLOYEE> ReadEmployee()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMPLOYEE.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadEmployee : {0} ", ex.Message));
                return null;
            }
        }
        /// <summary>
        /// Update Employee record
        /// </summary>
        /// <param name="modifiedEmployee">The modified employee.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateEmployee(HR_EMPLOYEE modifiedEmployee)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        HR_EMPLOYEE original = ctx.HR_EMPLOYEE.Find(modifiedEmployee.EMP_NUMBER);

                        if (original != null)
                        {
                            ctx.Entry(original).CurrentValues.SetValues(modifiedEmployee);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("Employee with EMP_NUMBER:{0}  was not found.", modifiedEmployee.EMP_NUMBER)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateEmployee : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Delete Employee record
        /// </summary>
        /// <param name="deletingEmployee">The deleting employee.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteEmployee(HR_EMPLOYEE deletingEmployee)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        HR_EMPLOYEE original = ctx.HR_EMPLOYEE.Find(deletingEmployee.EMP_NUMBER);

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
                                    string.Format("Employee with EMP_NUMBER:{0}  was not found.", deletingEmployee.EMP_NUMBER)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteEmployee) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Employees the exists.
        /// </summary>
        /// <param name="existsEmployee">The exists employee.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool EmployeeExists(HR_EMPLOYEE existsEmployee)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    HR_EMPLOYEE original = ctx.HR_EMPLOYEE.Find(existsEmployee.EMP_NUMBER);

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
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred Employee : {0} ", ex.Message));
                return false;
            }
        }
        /// <summary>
        /// Queries the employee.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;HR_EMPLOYEE&gt;.</returns>
        public ResultDTO<HR_EMPLOYEE> QueryEmployee(EmployeeQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.HR_EMPLOYEE.AsQueryable();

                    if (query.EmployeeNumber != null)
                    {
                        c = c.Where(i => i.EMP_NUMBER == query.EmployeeNumber);
                    }
                    if (query.EmployeeTitle != null)
                    {
                        c = c.Where(i => i.EMP_TITLE == query.EmployeeTitle);
                    }
                    if (query.EmployeeCallingName != null)
                    {
                        c = c.Where(i => i.EMP_CALLING_NAME == query.EmployeeCallingName);
                    }
                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.EMP_NUMBER).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<HR_EMPLOYEE>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on QueryEmployee : {0} ", ex.Message));
                return null;
            }
        }
        #endregion

        #region Employee_Personal_Details
        /// <summary>
        /// Creates the employee pd.
        /// </summary>
        /// <param name="new_HR_EMPLOYEE_PD">The new_ h r_ employe e_ pd.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateEmployeePD(HR_EMPLOYEE_PD new_HR_EMPLOYEE_PD)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        var Employee = ctx.HR_EMPLOYEE.Find(new_HR_EMPLOYEE_PD.HR_EMPLOYEE.EMP_NUMBER);
                        new_HR_EMPLOYEE_PD.HR_EMPLOYEE = Employee;

                        var Nationality = ctx.HR_NATIONALITY.Find(new_HR_EMPLOYEE_PD.HR_NATIONALITY.NAT_CODE);
                        new_HR_EMPLOYEE_PD.HR_NATIONALITY = Nationality;

                        var Religion = ctx.HR_RELIGION.Find(new_HR_EMPLOYEE_PD.HR_RELIGION.RLG_CODE);
                        new_HR_EMPLOYEE_PD.HR_RELIGION = Religion;

                        ctx.HR_EMPLOYEE_PD.Add(new_HR_EMPLOYEE_PD);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new EmployeePD : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Reads the employee p dby key.
        /// </summary>
        /// <param name="EMP_NUMBER_PD">The em p_ numbe r_ pd.</param>
        /// <returns>HR_EMPLOYEE_PD.</returns>
        public HR_EMPLOYEE_PD ReadEmployeePDbyKey(string EMP_NUMBER_PD)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMPLOYEE_PD.Where(c => c.EMP_NUMBER == EMP_NUMBER_PD).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadEmployeePDbyKey : {0} ", ex.Message));
                return null;
            }
        }
        /// <summary>
        /// Return all Employee Personal Details records
        /// </summary>
        /// <returns>HR_EMPLOYEE_PD[].</returns>
        public HR_EMPLOYEE_PD[] ReadAllEmployeePD()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMPLOYEE_PD.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAllEmployeePD : {0} ", ex.Message));
                return new HR_EMPLOYEE_PD[0];
            }
        }
        /// <summary>
        /// Execute the query and Return Employee Personal Details records
        /// </summary>
        /// <returns>IQueryable&lt;HR_EMPLOYEE_PD&gt;.</returns>
        public IQueryable<HR_EMPLOYEE_PD> ReadEmployeePD()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMPLOYEE_PD.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadEmployeePD : {0} ", ex.Message));
                return null;
            }
        }
        /// <summary>
        /// Update Employee Personal Details record
        /// </summary>
        /// <param name="modifiedEmployeePD">The modified employee pd.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateEmployeePD(HR_EMPLOYEE_PD modifiedEmployeePD)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        HR_EMPLOYEE_PD original = ctx.HR_EMPLOYEE_PD.Find(modifiedEmployeePD.EMP_NUMBER);

                        if (original != null)
                        {
                            var Employee = ctx.HR_EMPLOYEE.Find(modifiedEmployeePD.HR_EMPLOYEE.EMP_NUMBER);
                            modifiedEmployeePD.HR_EMPLOYEE = Employee;

                            var Nationality = ctx.HR_NATIONALITY.Find(modifiedEmployeePD.HR_NATIONALITY.NAT_CODE);
                            modifiedEmployeePD.HR_NATIONALITY = Nationality;

                            var Religion = ctx.HR_RELIGION.Find(modifiedEmployeePD.HR_RELIGION.RLG_CODE);
                            modifiedEmployeePD.HR_RELIGION = Religion;

                            ctx.Entry(original).CurrentValues.SetValues(modifiedEmployeePD);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("Employee Personal Details with EMP_NUMBER:{0}  was not found.", modifiedEmployeePD.EMP_NUMBER)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateEmployeePD : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Delete Employee Personal Details record
        /// </summary>
        /// <param name="deletingEmployeePD">The deleting employee pd.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteEmployeePD(HR_EMPLOYEE_PD deletingEmployeePD)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        HR_EMPLOYEE_PD original = ctx.HR_EMPLOYEE_PD.Find(deletingEmployeePD.EMP_NUMBER);

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
                                    string.Format("Employee with EMP_NUMBER:{0}  was not found.", deletingEmployeePD.EMP_NUMBER)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteEmployeePD) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Employees the pd exists.
        /// </summary>
        /// <param name="existsEmployeePD">The exists employee pd.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool EmployeePDExists(HR_EMPLOYEE_PD existsEmployeePD)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    HR_EMPLOYEE_PD original = ctx.HR_EMPLOYEE_PD.Find(existsEmployeePD.EMP_NUMBER);

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
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred EmployeePD : {0} ", ex.Message));
                return false;
            }
        }
        /// <summary>
        /// Queries the employee pd.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;HR_EMPLOYEE_PD&gt;.</returns>
        public ResultDTO<HR_EMPLOYEE_PD> QueryEmployeePD(EmployeePDQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.HR_EMPLOYEE_PD.AsQueryable();

                    if (query.EmployeeNumber != null)
                    {
                        c = c.Where(i => i.EMP_NUMBER == query.EmployeeNumber);
                    }
                    if (query.Gender != null)
                    {
                        c = c.Where(i => i.EMP_GENDER == query.Gender);
                    }
                    if (query.MaritalStatus != null)
                    {
                        c = c.Where(i => i.EMP_MARITAL_STATUS == query.MaritalStatus);
                    }
                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.EMP_NUMBER).Skip((pageNumber - 1) * pageSize).Take(pageSize).Include(s => s.HR_EMPLOYEE).Include(s => s.HR_NATIONALITY).Include(s => s.HR_RELIGION).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<HR_EMPLOYEE_PD>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on QueryEmployeePD : {0} ", ex.Message));
                return null;
            }
        }
        #endregion

        #region Employee_Job_Info
        /// <summary>
        /// Creates the employee ji.
        /// </summary>
        /// <param name="new_HR_EMPLOYEE_JI">The new_ h r_ employe e_ ji.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateEmployeeJI(HR_EMPLOYEE_JI new_HR_EMPLOYEE_JI)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        var Employee = ctx.HR_EMPLOYEE.Find(new_HR_EMPLOYEE_JI.HR_EMPLOYEE.EMP_NUMBER);
                        new_HR_EMPLOYEE_JI.HR_EMPLOYEE = Employee;

                        var Salary = ctx.HR_SALARY_GRADE.Find(new_HR_EMPLOYEE_JI.HR_SALARY_GRADE.SAL_GRD_CODE);
                        new_HR_EMPLOYEE_JI.HR_SALARY_GRADE = Salary;

                        var Corporate = ctx.HR_CORPORATE_TITLE.Find(new_HR_EMPLOYEE_JI.HR_CORPORATE_TITLE.CT_CODE);
                        new_HR_EMPLOYEE_JI.HR_CORPORATE_TITLE = Corporate;

                        var Designation = ctx.HR_DESIGNATION.Find(new_HR_EMPLOYEE_JI.HR_DESIGNATION.DSG_CODE);
                        new_HR_EMPLOYEE_JI.HR_DESIGNATION = Designation;

                        ctx.HR_EMPLOYEE_JI.Add(new_HR_EMPLOYEE_JI);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new EmployeeJI : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Reads the employee j iby key.
        /// </summary>
        /// <param name="EMP_NUMBER_JI">The em p_ numbe r_ ji.</param>
        /// <returns>HR_EMPLOYEE_JI.</returns>
        public HR_EMPLOYEE_JI ReadEmployeeJIbyKey(string EMP_NUMBER_JI)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMPLOYEE_JI.Where(c => c.EMP_NUMBER == EMP_NUMBER_JI).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadEmployeeJIbyKey : {0} ", ex.Message));
                return null;
            }
        }
        /// <summary>
        /// Return all Employee Job Infomation records
        /// </summary>
        /// <returns>HR_EMPLOYEE_JI[].</returns>
        public HR_EMPLOYEE_JI[] ReadAllEmployeeJI()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMPLOYEE_JI.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAllEmployeeJI : {0} ", ex.Message));
                return new HR_EMPLOYEE_JI[0];
            }
        }
        /// <summary>
        /// Execute the query and Return Employee Job Infomation records
        /// </summary>
        /// <returns>IQueryable&lt;HR_EMPLOYEE_JI&gt;.</returns>
        public IQueryable<HR_EMPLOYEE_JI> ReadEmployeeJI()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMPLOYEE_JI.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadEmployeeJI : {0} ", ex.Message));
                return null;
            }
        }
        /// <summary>
        /// Update Employee Job Infomation record
        /// </summary>
        /// <param name="modifiedEmployeeJI">The modified employee ji.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateEmployeeJI(HR_EMPLOYEE_JI modifiedEmployeeJI)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        HR_EMPLOYEE_JI original = ctx.HR_EMPLOYEE_JI.Find(modifiedEmployeeJI.EMP_NUMBER);

                        if (original != null)
                        {
                            var Employee = ctx.HR_EMPLOYEE.Find(modifiedEmployeeJI.HR_EMPLOYEE.EMP_NUMBER);
                            modifiedEmployeeJI.HR_EMPLOYEE = Employee;

                            var Salary = ctx.HR_SALARY_GRADE.Find(modifiedEmployeeJI.HR_SALARY_GRADE.SAL_GRD_CODE);
                            modifiedEmployeeJI.HR_SALARY_GRADE = Salary;

                            var Corporate = ctx.HR_CORPORATE_TITLE.Find(modifiedEmployeeJI.HR_CORPORATE_TITLE.CT_CODE);
                            modifiedEmployeeJI.HR_CORPORATE_TITLE = Corporate;

                            var Designation = ctx.HR_DESIGNATION.Find(modifiedEmployeeJI.HR_DESIGNATION.DSG_CODE);
                            modifiedEmployeeJI.HR_DESIGNATION = Designation;

                            ctx.Entry(original).CurrentValues.SetValues(modifiedEmployeeJI);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("Employee with EMP_NUMBER:{0}  was not found.", modifiedEmployeeJI.EMP_NUMBER)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateEmployeeJI : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Delete Employee Job Infomation record
        /// </summary>
        /// <param name="deletingEmployeeJI">The deleting employee ji.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteEmployeeJI(HR_EMPLOYEE_JI deletingEmployeeJI)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        HR_EMPLOYEE_JI original = ctx.HR_EMPLOYEE_JI.Find(deletingEmployeeJI.EMP_NUMBER);

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
                                    string.Format("Employee with EMP_NUMBER:{0}  was not found.", deletingEmployeeJI.EMP_NUMBER)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteEmployeeJI) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Employees the ji exists.
        /// </summary>
        /// <param name="existsEmployeeJI">The exists employee ji.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool EmployeeJIExists(HR_EMPLOYEE_JI existsEmployeeJI)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    HR_EMPLOYEE_JI original = ctx.HR_EMPLOYEE_JI.Find(existsEmployeeJI.EMP_NUMBER);

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
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred EmployeeJI : {0} ", ex.Message));
                return false;
            }
        }
        /// <summary>
        /// Queries the employee ji.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;HR_EMPLOYEE_JI&gt;.</returns>
        public ResultDTO<HR_EMPLOYEE_JI> QueryEmployeeJI(EmployeeJIQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.HR_EMPLOYEE_JI.AsQueryable();

                    if (query.EmployeeNumber != null)
                    {
                        c = c.Where(i => i.EMP_NUMBER == query.EmployeeNumber);
                    }
                    if (query.WorkingHours > 0.0)
                    {
                        c = c.Where(i => i.EMP_WORKHOURS == query.WorkingHours);
                    }
                    if (query.Confirm > 0)
                    {
                        c = c.Where(i => i.EMP_CONFIRM_FLG == query.Confirm);
                    }
                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.EMP_NUMBER).Skip((pageNumber - 1) * pageSize).Take(pageSize).Include(s => s.HR_EMPLOYEE).Include(s => s.HR_SALARY_GRADE).Include(s => s.HR_CORPORATE_TITLE).Include(s => s.HR_DESIGNATION).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<HR_EMPLOYEE_JI>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on QueryEmployeeJI : {0} ", ex.Message));
                return null;
            }
        }

        #endregion

        #region Employee_Job_Status
        /// <summary>
        /// Creates the employee js.
        /// </summary>
        /// <param name="new_HR_EMPLOYEE_JS">The new_ h r_ employe e_ js.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateEmployeeJS(HR_EMPLOYEE_JS new_HR_EMPLOYEE_JS)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        var Employee = ctx.HR_EMPLOYEE.Find(new_HR_EMPLOYEE_JS.HR_EMPLOYEE.EMP_NUMBER);
                        new_HR_EMPLOYEE_JS.HR_EMPLOYEE = Employee;

                        var Category = ctx.HR_JD_CATEGORY.Find(new_HR_EMPLOYEE_JS.HR_JD_CATEGORY.JDCAT_CODE);
                        new_HR_EMPLOYEE_JS.HR_JD_CATEGORY = Category;

                        var Staff = ctx.HR_JD_CATEGORY.Find(new_HR_EMPLOYEE_JS.HR_JD_CATEGORY1.JDCAT_CODE);
                        new_HR_EMPLOYEE_JS.HR_JD_CATEGORY1 = Staff;

                        ctx.HR_EMPLOYEE_JS.Add(new_HR_EMPLOYEE_JS);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new EmployeeJS : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Reads the employee j sby key.
        /// </summary>
        /// <param name="EMP_NUMBER_JS">The em p_ numbe r_ js.</param>
        /// <returns>HR_EMPLOYEE_JS.</returns>
        public HR_EMPLOYEE_JS ReadEmployeeJSbyKey(string EMP_NUMBER_JS)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMPLOYEE_JS.Where(c => c.EMP_NUMBER == EMP_NUMBER_JS).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadEmployeeJSbyKey : {0} ", ex.Message));
                return null;
            }
        }
        /// <summary>
        /// Return all Employee Job Status records
        /// </summary>
        /// <returns>HR_EMPLOYEE_JS[].</returns>
        public HR_EMPLOYEE_JS[] ReadAllEmployeeJS()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMPLOYEE_JS.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAllEmployeeJS : {0} ", ex.Message));
                return new HR_EMPLOYEE_JS[0];
            }
        }
        /// <summary>
        /// Execute the query and Return Employee Job Status records
        /// </summary>
        /// <returns>IQueryable&lt;HR_EMPLOYEE_JS&gt;.</returns>
        public IQueryable<HR_EMPLOYEE_JS> ReadEmployeeJS()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMPLOYEE_JS.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadEmployeeJS : {0} ", ex.Message));
                return null;
            }
        }
        /// <summary>
        /// Update Employee Job Status record
        /// </summary>
        /// <param name="modifiedEmployeeJS">The modified employee js.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateEmployeeJS(HR_EMPLOYEE_JS modifiedEmployeeJS)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        HR_EMPLOYEE_JS original = ctx.HR_EMPLOYEE_JS.Find(modifiedEmployeeJS.EMP_NUMBER);

                        if (original != null)
                        {
                            var Employee = ctx.HR_EMPLOYEE.Find(modifiedEmployeeJS.HR_EMPLOYEE.EMP_NUMBER);
                            modifiedEmployeeJS.HR_EMPLOYEE = Employee;

                            var Category = ctx.HR_JD_CATEGORY.Find(modifiedEmployeeJS.HR_JD_CATEGORY.JDCAT_CODE);
                            modifiedEmployeeJS.HR_JD_CATEGORY = Category;

                            var Staff = ctx.HR_JD_CATEGORY.Find(modifiedEmployeeJS.HR_JD_CATEGORY1.JDCAT_CODE);
                            modifiedEmployeeJS.HR_JD_CATEGORY1 = Staff;

                            ctx.Entry(original).CurrentValues.SetValues(modifiedEmployeeJS);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("Employee with EMP_NUMBER:{0}  was not found.", modifiedEmployeeJS.EMP_NUMBER)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateEmployeeJS : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Delete Employee Job Status record
        /// </summary>
        /// <param name="deletingEmployeeJS">The deleting employee js.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteEmployeeJS(HR_EMPLOYEE_JS deletingEmployeeJS)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        HR_EMPLOYEE_JS original = ctx.HR_EMPLOYEE_JS.Find(deletingEmployeeJS.EMP_NUMBER);

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
                                    string.Format("Employee with EMP_NUMBER:{0}  was not found.", deletingEmployeeJS.EMP_NUMBER)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteEmployeeJS) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Employees the js exists.
        /// </summary>
        /// <param name="existsEmployeeJS">The exists employee js.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool EmployeeJSExists(HR_EMPLOYEE_JS existsEmployeeJS)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    HR_EMPLOYEE_JS original = ctx.HR_EMPLOYEE_JS.Find(existsEmployeeJS.EMP_NUMBER);

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
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred EmployeeJS : {0} ", ex.Message));
                return false;
            }
        }
        /// <summary>
        /// Queries the employee js.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;HR_EMPLOYEE_JS&gt;.</returns>
        public ResultDTO<HR_EMPLOYEE_JS> QueryEmployeeJS(EmployeeJSQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.HR_EMPLOYEE_JS.AsQueryable();

                    if (query.EmployeeNumber != null)
                    {
                        c = c.Where(i => i.EMP_NUMBER == query.EmployeeNumber);
                    }
                    if (query.EmployeeType != null)
                    {
                        c = c.Where(i => i.EMP_TYPE == query.EmployeeType);
                    }
                    if (query.EmployeeCategory != null)
                    {
                        c = c.Where(i => i.CAT_CODE == query.EmployeeCategory);
                    }
                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.EMP_NUMBER).Skip((pageNumber - 1) * pageSize).Take(pageSize).Include(s => s.HR_EMPLOYEE).Include(s => s.HR_JD_CATEGORY).Include(s => s.HR_JD_CATEGORY1).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<HR_EMPLOYEE_JS>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on QueryEmployeeJS : {0} ", ex.Message));
                return null;
            }
        }

        #endregion

        #region Employee_Tax_Details
        /// <summary>
        /// Creates the employee td.
        /// </summary>
        /// <param name="new_HR_EMPLOYEE_TD">The new_ h r_ employe e_ td.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateEmployeeTD(HR_EMPLOYEE_TD new_HR_EMPLOYEE_TD)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        var Employee = ctx.HR_EMPLOYEE.Find(new_HR_EMPLOYEE_TD.HR_EMPLOYEE.EMP_NUMBER);
                        new_HR_EMPLOYEE_TD.HR_EMPLOYEE = Employee;

                        ctx.HR_EMPLOYEE_TD.Add(new_HR_EMPLOYEE_TD);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new EmployeeTD : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Reads the employee t dby key.
        /// </summary>
        /// <param name="EMP_NUMBER_TD">The em p_ numbe r_ td.</param>
        /// <returns>HR_EMPLOYEE_TD.</returns>
        public HR_EMPLOYEE_TD ReadEmployeeTDbyKey(string EMP_NUMBER_TD)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMPLOYEE_TD.Where(c => c.EMP_NUMBER == EMP_NUMBER_TD).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadEmployeeTDbyKey : {0} ", ex.Message));
                return null;
            }
        }
        /// <summary>
        /// Return all Employee Personal Details records
        /// </summary>
        /// <returns>HR_EMPLOYEE_TD[].</returns>
        public HR_EMPLOYEE_TD[] ReadAllEmployeeTD()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMPLOYEE_TD.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAllEmployeeTD : {0} ", ex.Message));
                return new HR_EMPLOYEE_TD[0];
            }
        }
        /// <summary>
        /// Execute the query and Return Employee Tax Details records
        /// </summary>
        /// <returns>IQueryable&lt;HR_EMPLOYEE_TD&gt;.</returns>
        public IQueryable<HR_EMPLOYEE_TD> ReadEmployeeTD()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMPLOYEE_TD.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadEmployeeTD : {0} ", ex.Message));
                return null;
            }
        }
        /// <summary>
        /// Update Employee Tax Details record
        /// </summary>
        /// <param name="modifiedEmployeeTD">The modified employee td.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateEmployeeTD(HR_EMPLOYEE_TD modifiedEmployeeTD)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        HR_EMPLOYEE_TD original = ctx.HR_EMPLOYEE_TD.Find(modifiedEmployeeTD.EMP_NUMBER);

                        if (original != null)
                        {
                            var Employee = ctx.HR_EMPLOYEE.Find(modifiedEmployeeTD.HR_EMPLOYEE.EMP_NUMBER);
                            modifiedEmployeeTD.HR_EMPLOYEE = Employee;

                            ctx.Entry(original).CurrentValues.SetValues(modifiedEmployeeTD);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("Employee with EMP_NUMBER:{0}  was not found.", modifiedEmployeeTD.EMP_NUMBER)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateEmployeeTD : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Delete Employee Tax Details record
        /// </summary>
        /// <param name="deletingEmployeeTD">The deleting employee td.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteEmployeeTD(HR_EMPLOYEE_TD deletingEmployeeTD)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        HR_EMPLOYEE_TD original = ctx.HR_EMPLOYEE_TD.Find(deletingEmployeeTD.EMP_NUMBER);

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
                                    string.Format("Employee with EMP_NUMBER:{0}  was not found.", deletingEmployeeTD.EMP_NUMBER)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteEmployeeTD) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Employees the td exists.
        /// </summary>
        /// <param name="existsEmployeeTD">The exists employee td.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool EmployeeTDExists(HR_EMPLOYEE_TD existsEmployeeTD)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    HR_EMPLOYEE_TD original = ctx.HR_EMPLOYEE_TD.Find(existsEmployeeTD.EMP_NUMBER);

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
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred EmployeeTD : {0} ", ex.Message));
                return false;
            }
        }
        /// <summary>
        /// Queries the employee td.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;HR_EMPLOYEE_TD&gt;.</returns>
        public ResultDTO<HR_EMPLOYEE_TD> QueryEmployeeTD(EmployeeTDQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.HR_EMPLOYEE_TD.AsQueryable();

                    if (query.EmployeeNumber != null)
                    {
                        c = c.Where(i => i.EMP_NUMBER == query.EmployeeNumber);
                    }
                    if (query.EmployeeExempt != null)
                    {
                        c = c.Where(i => i.EMP_PAYE_TAX_EXEMPT == query.EmployeeExempt);
                    }
                    if (query.EmployeeFlag != null)
                    {
                        c = c.Where(i => i.EMP_TAXONTAX_FLG == query.EmployeeFlag);
                    }
                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.EMP_NUMBER).Skip((pageNumber - 1) * pageSize).Take(pageSize).Include(s => s.HR_EMPLOYEE).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<HR_EMPLOYEE_TD>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on QueryEmployeeTD : {0} ", ex.Message));
                return null;
            }
        }

        #endregion

        #region Employee_Permanent_Contacts
        /// <summary>
        /// Creates the employee pc.
        /// </summary>
        /// <param name="new_HR_EMPLOYEE_PC">The new_ h r_ employe e_ pc.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateEmployeePC(HR_EMPLOYEE_PC new_HR_EMPLOYEE_PC)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        var Employee = ctx.HR_EMPLOYEE.Find(new_HR_EMPLOYEE_PC.HR_EMPLOYEE.EMP_NUMBER);
                        new_HR_EMPLOYEE_PC.HR_EMPLOYEE = Employee;

                        var Country = ctx.HR_COUNTRY.Find(new_HR_EMPLOYEE_PC.HR_COUNTRY.COU_CODE);
                        new_HR_EMPLOYEE_PC.HR_COUNTRY = Country;

                        var Province = ctx.HR_PROVINCE.Find(new_HR_EMPLOYEE_PC.HR_PROVINCE.PROVINCE_CODE);
                        new_HR_EMPLOYEE_PC.HR_PROVINCE = Province;

                        var District = ctx.HR_DISTRICT.Find(new_HR_EMPLOYEE_PC.HR_DISTRICT.DISTRICT_CODE);
                        new_HR_EMPLOYEE_PC.HR_DISTRICT = District;

                        var Electorate = ctx.HR_ELECTORATE.Find(new_HR_EMPLOYEE_PC.HR_ELECTORATE.ELECTORATE_CODE);
                        new_HR_EMPLOYEE_PC.HR_ELECTORATE = Electorate;

                        ctx.HR_EMPLOYEE_PC.Add(new_HR_EMPLOYEE_PC);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new EmployeePC : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Reads the employee p cby key.
        /// </summary>
        /// <param name="EMP_NUMBER_PC">The em p_ numbe r_ pc.</param>
        /// <returns>HR_EMPLOYEE_PC.</returns>
        public HR_EMPLOYEE_PC ReadEmployeePCbyKey(string EMP_NUMBER_PC)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMPLOYEE_PC.Where(c => c.EMP_NUMBER == EMP_NUMBER_PC).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadEmployeePCbyKey : {0} ", ex.Message));
                return null;
            }
        }
        /// <summary>
        /// Return all Employee Permanent Contact records
        /// </summary>
        /// <returns>HR_EMPLOYEE_PC[].</returns>
        public HR_EMPLOYEE_PC[] ReadAllEmployeePC()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMPLOYEE_PC.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAllEmployeePC : {0} ", ex.Message));
                return new HR_EMPLOYEE_PC[0];
            }
        }
        /// <summary>
        /// Execute the query and Return Employee Permanent Contact records
        /// </summary>
        /// <returns>IQueryable&lt;HR_EMPLOYEE_PC&gt;.</returns>
        public IQueryable<HR_EMPLOYEE_PC> ReadEmployeePC()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMPLOYEE_PC.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadEmployeePC : {0} ", ex.Message));
                return null;
            }
        }
        /// <summary>
        /// Update Employee Permanent Contact record
        /// </summary>
        /// <param name="modifiedEmployeePC">The modified employee pc.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateEmployeePC(HR_EMPLOYEE_PC modifiedEmployeePC)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        HR_EMPLOYEE_PC original = ctx.HR_EMPLOYEE_PC.Find(modifiedEmployeePC.EMP_NUMBER);

                        if (original != null)
                        {
                            var Employee = ctx.HR_EMPLOYEE.Find(modifiedEmployeePC.HR_EMPLOYEE.EMP_NUMBER);
                            modifiedEmployeePC.HR_EMPLOYEE = Employee;

                            var Country = ctx.HR_COUNTRY.Find(modifiedEmployeePC.HR_COUNTRY.COU_CODE);
                            modifiedEmployeePC.HR_COUNTRY = Country;

                            var Province = ctx.HR_PROVINCE.Find(modifiedEmployeePC.HR_PROVINCE.PROVINCE_CODE);
                            modifiedEmployeePC.HR_PROVINCE = Province;

                            var District = ctx.HR_DISTRICT.Find(modifiedEmployeePC.HR_DISTRICT.DISTRICT_CODE);
                            modifiedEmployeePC.HR_DISTRICT = District;

                            var Electorate = ctx.HR_ELECTORATE.Find(modifiedEmployeePC.HR_ELECTORATE.ELECTORATE_CODE);
                            modifiedEmployeePC.HR_ELECTORATE = Electorate;

                            ctx.Entry(original).CurrentValues.SetValues(modifiedEmployeePC);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("Employee with EMP_NUMBER:{0}  was not found.", modifiedEmployeePC.EMP_NUMBER)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateEmployeePC : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Delete Employee Permanent Contact record
        /// </summary>
        /// <param name="deletingEmployeePC">The deleting employee pc.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteEmployeePC(HR_EMPLOYEE_PC deletingEmployeePC)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        HR_EMPLOYEE_PC original = ctx.HR_EMPLOYEE_PC.Find(deletingEmployeePC.EMP_NUMBER);

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
                                    string.Format("Employee with EMP_NUMBER:{0}  was not found.", deletingEmployeePC.EMP_NUMBER)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteEmployeePC) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Employees the pc exists.
        /// </summary>
        /// <param name="existsEmployeePC">The exists employee pc.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool EmployeePCExists(HR_EMPLOYEE_PC existsEmployeePC)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    HR_EMPLOYEE_PC original = ctx.HR_EMPLOYEE_PC.Find(existsEmployeePC.EMP_NUMBER);

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
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred EmployeePC : {0} ", ex.Message));
                return false;
            }
        }
        /// <summary>
        /// Queries the employee pc.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;HR_EMPLOYEE_PC&gt;.</returns>
        public ResultDTO<HR_EMPLOYEE_PC> QueryEmployeePC(EmployeePCQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.HR_EMPLOYEE_PC.AsQueryable();

                    if (query.EmployeeNumber != null)
                    {
                        c = c.Where(i => i.EMP_NUMBER == query.EmployeeNumber);
                    }
                    if (query.City != null)
                    {
                        c = c.Where(i => i.EMP_PER_CITY == query.City);
                    }
                    if (query.Province != null)
                    {
                        c = c.Where(i => i.EMP_PER_PROVINCE_CODE == query.Province);
                    }
                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.EMP_NUMBER).Skip((pageNumber - 1) * pageSize).Take(pageSize).Include(s => s.HR_EMPLOYEE).Include(s => s.HR_COUNTRY).Include(s => s.HR_PROVINCE).Include(s => s.HR_DISTRICT).Include(s => s.HR_ELECTORATE).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<HR_EMPLOYEE_PC>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on QueryEmployeePC : {0} ", ex.Message));
                return null;
            }
        }

        #endregion

        #region Employee_Contacts_during_working_days
        /// <summary>
        /// Creates the employee CDWD.
        /// </summary>
        /// <param name="new_HR_EMPLOYEE_CDWD">The new_ h r_ employe e_ CDWD.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateEmployeeCDWD(HR_EMPLOYEE_CDWD new_HR_EMPLOYEE_CDWD)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        var Employee = ctx.HR_EMPLOYEE.Find(new_HR_EMPLOYEE_CDWD.HR_EMPLOYEE.EMP_NUMBER);
                        new_HR_EMPLOYEE_CDWD.HR_EMPLOYEE = Employee;

                        var Country = ctx.HR_COUNTRY.Find(new_HR_EMPLOYEE_CDWD.HR_COUNTRY.COU_CODE);
                        new_HR_EMPLOYEE_CDWD.HR_COUNTRY = Country;

                        var Province = ctx.HR_PROVINCE.Find(new_HR_EMPLOYEE_CDWD.HR_PROVINCE.PROVINCE_CODE);
                        new_HR_EMPLOYEE_CDWD.HR_PROVINCE = Province;

                        var District = ctx.HR_DISTRICT.Find(new_HR_EMPLOYEE_CDWD.HR_DISTRICT.DISTRICT_CODE);
                        new_HR_EMPLOYEE_CDWD.HR_DISTRICT = District;

                        var Electorate = ctx.HR_ELECTORATE.Find(new_HR_EMPLOYEE_CDWD.HR_ELECTORATE.ELECTORATE_CODE);
                        new_HR_EMPLOYEE_CDWD.HR_ELECTORATE = Electorate;

                        ctx.HR_EMPLOYEE_CDWD.Add(new_HR_EMPLOYEE_CDWD);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new EmployeeCDWD : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Reads the employee CDW dby key.
        /// </summary>
        /// <param name="EMP_NUMBER_CDWD">The em p_ numbe r_ CDWD.</param>
        /// <returns>HR_EMPLOYEE_CDWD.</returns>
        public HR_EMPLOYEE_CDWD ReadEmployeeCDWDbyKey(string EMP_NUMBER_CDWD)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMPLOYEE_CDWD.Where(c => c.EMP_NUMBER == EMP_NUMBER_CDWD).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadEmployeeCDWDbyKey : {0} ", ex.Message));
                return null;
            }
        }
        /// <summary>
        /// Return all Employee Contacts during working days records
        /// </summary>
        /// <returns>HR_EMPLOYEE_CDWD[].</returns>
        public HR_EMPLOYEE_CDWD[] ReadAllEmployeeCDWD()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMPLOYEE_CDWD.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAllEmployeeCDWD : {0} ", ex.Message));
                return new HR_EMPLOYEE_CDWD[0];
            }
        }
        /// <summary>
        /// Execute the query and Return Employee Contacts during working days records
        /// </summary>
        /// <returns>IQueryable&lt;HR_EMPLOYEE_CDWD&gt;.</returns>
        public IQueryable<HR_EMPLOYEE_CDWD> ReadEmployeeCDWD()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMPLOYEE_CDWD.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadEmployeeCDWD : {0} ", ex.Message));
                return null;
            }
        }
        /// <summary>
        /// Update Employee Personal Details record
        /// </summary>
        /// <param name="modifiedEmployeeCDWD">The modified employee CDWD.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateEmployeeCDWD(HR_EMPLOYEE_CDWD modifiedEmployeeCDWD)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        HR_EMPLOYEE_CDWD original = ctx.HR_EMPLOYEE_CDWD.Find(modifiedEmployeeCDWD.EMP_NUMBER);

                        if (original != null)
                        {
                            var Employee = ctx.HR_EMPLOYEE.Find(modifiedEmployeeCDWD.HR_EMPLOYEE.EMP_NUMBER);
                            modifiedEmployeeCDWD.HR_EMPLOYEE = Employee;

                            var Country = ctx.HR_COUNTRY.Find(modifiedEmployeeCDWD.HR_COUNTRY.COU_CODE);
                            modifiedEmployeeCDWD.HR_COUNTRY = Country;

                            var Province = ctx.HR_PROVINCE.Find(modifiedEmployeeCDWD.HR_PROVINCE.PROVINCE_CODE);
                            modifiedEmployeeCDWD.HR_PROVINCE = Province;

                            var District = ctx.HR_DISTRICT.Find(modifiedEmployeeCDWD.HR_DISTRICT.DISTRICT_CODE);
                            modifiedEmployeeCDWD.HR_DISTRICT = District;

                            var Electorate = ctx.HR_ELECTORATE.Find(modifiedEmployeeCDWD.HR_ELECTORATE.ELECTORATE_CODE);
                            modifiedEmployeeCDWD.HR_ELECTORATE = Electorate;

                            ctx.Entry(original).CurrentValues.SetValues(modifiedEmployeeCDWD);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("Employee with EMP_NUMBER:{0}  was not found.", modifiedEmployeeCDWD.EMP_NUMBER)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateEmployeeCDWD : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Delete Employee Personal Details record
        /// </summary>
        /// <param name="deletingEmployeeCDWD">The deleting employee CDWD.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteEmployeeCDWD(HR_EMPLOYEE_CDWD deletingEmployeeCDWD)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        HR_EMPLOYEE_CDWD original = ctx.HR_EMPLOYEE_CDWD.Find(deletingEmployeeCDWD.EMP_NUMBER);

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
                                    string.Format("Employee with EMP_NUMBER:{0}  was not found.", deletingEmployeeCDWD.EMP_NUMBER)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteEmployeeCDWD) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Employees the CDWD exists.
        /// </summary>
        /// <param name="existsEmployeeCDWD">The exists employee CDWD.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool EmployeeCDWDExists(HR_EMPLOYEE_CDWD existsEmployeeCDWD)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    HR_EMPLOYEE_CDWD original = ctx.HR_EMPLOYEE_CDWD.Find(existsEmployeeCDWD.EMP_NUMBER);

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
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred EmployeeCDWD : {0} ", ex.Message));
                return false;
            }
        }
        /// <summary>
        /// Queries the employee CDWD.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;HR_EMPLOYEE_CDWD&gt;.</returns>
        public ResultDTO<HR_EMPLOYEE_CDWD> QueryEmployeeCDWD(EmployeeCDWDQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.HR_EMPLOYEE_CDWD.AsQueryable();

                    if (query.EmployeeNumber != null)
                    {
                        c = c.Where(i => i.EMP_NUMBER == query.EmployeeNumber);
                    }
                    if (query.City != null)
                    {
                        c = c.Where(i => i.EMP_TEM_CITY == query.City);
                    }
                    if (query.Province != null)
                    {
                        c = c.Where(i => i.EMP_TEM_PROVINCE_CODE == query.Province);
                    }
                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.EMP_NUMBER).Skip((pageNumber - 1) * pageSize).Take(pageSize).Include(s => s.HR_EMPLOYEE).Include(s => s.HR_COUNTRY).Include(s => s.HR_PROVINCE).Include(s => s.HR_DISTRICT).Include(s => s.HR_ELECTORATE).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<HR_EMPLOYEE_CDWD>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on QueryEmployeeCDWD : {0} ", ex.Message));
                return null;
            }
        }

        #endregion

        #region Employee_Official_contacts
        /// <summary>
        /// Creates the employee oc.
        /// </summary>
        /// <param name="new_HR_EMPLOYEE_OC">The new_ h r_ employe e_ oc.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateEmployeeOC(HR_EMPLOYEE_OC new_HR_EMPLOYEE_OC)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        var Employee = ctx.HR_EMPLOYEE.Find(new_HR_EMPLOYEE_OC.HR_EMPLOYEE.EMP_NUMBER);
                        new_HR_EMPLOYEE_OC.HR_EMPLOYEE = Employee;

                        ctx.HR_EMPLOYEE_OC.Add(new_HR_EMPLOYEE_OC);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new EmployeeOC : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Reads the employee o cby key.
        /// </summary>
        /// <param name="EMP_NUMBER_OC">The em p_ numbe r_ oc.</param>
        /// <returns>HR_EMPLOYEE_OC.</returns>
        public HR_EMPLOYEE_OC ReadEmployeeOCbyKey(string EMP_NUMBER_OC)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMPLOYEE_OC.Where(c => c.EMP_NUMBER == EMP_NUMBER_OC).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadEmployeeOCbyKey : {0} ", ex.Message));
                return null;
            }
        }
        /// <summary>
        /// Return all Employee Official contacts records
        /// </summary>
        /// <returns>HR_EMPLOYEE_OC[].</returns>
        public HR_EMPLOYEE_OC[] ReadAllEmployeeOC()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMPLOYEE_OC.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAllEmployeeOC : {0} ", ex.Message));
                return new HR_EMPLOYEE_OC[0];
            }
        }
        /// <summary>
        /// Execute the query and Return Employee Official contacts records
        /// </summary>
        /// <returns>IQueryable&lt;HR_EMPLOYEE_OC&gt;.</returns>
        public IQueryable<HR_EMPLOYEE_OC> ReadEmployeeOC()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMPLOYEE_OC.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadEmployeeOC : {0} ", ex.Message));
                return null;
            }
        }
        /// <summary>
        /// Update Employee Official contacts record
        /// </summary>
        /// <param name="modifiedEmployeeOC">The modified employee oc.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateEmployeeOC(HR_EMPLOYEE_OC modifiedEmployeeOC)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        HR_EMPLOYEE_OC original = ctx.HR_EMPLOYEE_OC.Find(modifiedEmployeeOC.EMP_NUMBER);

                        if (original != null)
                        {
                            var Employee = ctx.HR_EMPLOYEE.Find(modifiedEmployeeOC.HR_EMPLOYEE.EMP_NUMBER);
                            modifiedEmployeeOC.HR_EMPLOYEE = Employee;

                            ctx.Entry(original).CurrentValues.SetValues(modifiedEmployeeOC);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("Employee with EMP_NUMBER:{0}  was not found.", modifiedEmployeeOC.EMP_NUMBER)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateEmployeeOC : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Delete Employee Official contacts record
        /// </summary>
        /// <param name="deletingEmployeeOC">The deleting employee oc.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteEmployeeOC(HR_EMPLOYEE_OC deletingEmployeeOC)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        HR_EMPLOYEE_OC original = ctx.HR_EMPLOYEE_OC.Find(deletingEmployeeOC.EMP_NUMBER);

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
                                    string.Format("Employee with EMP_NUMBER:{0}  was not found.", deletingEmployeeOC.EMP_NUMBER)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteEmployeeOC) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Employees the oc exists.
        /// </summary>
        /// <param name="existsEmployeeOC">The exists employee oc.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool EmployeeOCExists(HR_EMPLOYEE_OC existsEmployeeOC)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    HR_EMPLOYEE_OC original = ctx.HR_EMPLOYEE_OC.Find(existsEmployeeOC.EMP_NUMBER);

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
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred EmployeeOC : {0} ", ex.Message));
                return false;
            }
        }
        /// <summary>
        /// Queries the employee oc.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;HR_EMPLOYEE_OC&gt;.</returns>
        public ResultDTO<HR_EMPLOYEE_OC> QueryEmployeeOC(EmployeeOCQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.HR_EMPLOYEE_OC.AsQueryable();

                    if (query.EmployeeNumber != null)
                    {
                        c = c.Where(i => i.EMP_NUMBER == query.EmployeeNumber);
                    }
                    if (query.OfficeExtension != null)
                    {
                        c = c.Where(i => i.EMP_OFFICE_EXTN == query.OfficeExtension);
                    }
                    if (query.Phone != null)
                    {
                        c = c.Where(i => i.EMP_OFFICE_PHONE == query.Phone);
                    }
                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.EMP_NUMBER).Skip((pageNumber - 1) * pageSize).Take(pageSize).Include(s => s.HR_EMPLOYEE).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<HR_EMPLOYEE_OC>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on QueryEmployeeOC : {0} ", ex.Message));
                return null;
            }
        }

        #endregion

        #region Employee_EIM_Work_Station_Details
        /// <summary>
        /// Creates the employee eimws.
        /// </summary>
        /// <param name="new_HR_EMPLOYEE_EIM_WS">The new_ h r_ employe e_ ei m_ ws.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateEmployeeEIMWS(HR_EMPLOYEE_EIM_WS new_HR_EMPLOYEE_EIM_WS)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        var Employee = ctx.HR_EMPLOYEE.Find(new_HR_EMPLOYEE_EIM_WS.HR_EMPLOYEE.EMP_NUMBER);
                        new_HR_EMPLOYEE_EIM_WS.HR_EMPLOYEE = Employee;

                        var Location = ctx.HR_LOCATION.Find(new_HR_EMPLOYEE_EIM_WS.HR_LOCATION.LOC_CODE);
                        new_HR_EMPLOYEE_EIM_WS.HR_LOCATION = Location;

                        ctx.HR_EMPLOYEE_EIM_WS.Add(new_HR_EMPLOYEE_EIM_WS);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new EmployeeEIMWS : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Reads the employee eimw sby key.
        /// </summary>
        /// <param name="EMP_NUMBER_EIM_WS">The em p_ numbe r_ ei m_ ws.</param>
        /// <returns>HR_EMPLOYEE_EIM_WS.</returns>
        public HR_EMPLOYEE_EIM_WS ReadEmployeeEIMWSbyKey(string EMP_NUMBER_EIM_WS)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMPLOYEE_EIM_WS.Where(c => c.EMP_NUMBER == EMP_NUMBER_EIM_WS).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadEmployeeEIMWSbyKey : {0} ", ex.Message));
                return null;
            }
        }
        /// <summary>
        /// Return all Employee EIM Work Station Details records
        /// </summary>
        /// <returns>HR_EMPLOYEE_EIM_WS[].</returns>
        public HR_EMPLOYEE_EIM_WS[] ReadAllEmployeeEIMWS()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMPLOYEE_EIM_WS.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAllEmployeeEIMWS : {0} ", ex.Message));
                return new HR_EMPLOYEE_EIM_WS[0];
            }
        }
        /// <summary>
        /// Execute the query and Return Employee EIM Work Station Details records
        /// </summary>
        /// <returns>IQueryable&lt;HR_EMPLOYEE_EIM_WS&gt;.</returns>
        public IQueryable<HR_EMPLOYEE_EIM_WS> ReadEmployeeEIMWS()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMPLOYEE_EIM_WS.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadEmployeeEIMWS : {0} ", ex.Message));
                return null;
            }
        }
        /// <summary>
        /// Update Employee EIM Work Station Details record
        /// </summary>
        /// <param name="modifiedEmployeeEIMWS">The modified employee eimws.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateEmployeeEIMWS(HR_EMPLOYEE_EIM_WS modifiedEmployeeEIMWS)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        HR_EMPLOYEE_EIM_WS original = ctx.HR_EMPLOYEE_EIM_WS.Find(modifiedEmployeeEIMWS.EMP_NUMBER);

                        if (original != null)
                        {
                            var Employee = ctx.HR_EMPLOYEE.Find(modifiedEmployeeEIMWS.HR_EMPLOYEE.EMP_NUMBER);
                            modifiedEmployeeEIMWS.HR_EMPLOYEE = Employee;

                            var Location = ctx.HR_LOCATION.Find(modifiedEmployeeEIMWS.HR_LOCATION.LOC_CODE);
                            modifiedEmployeeEIMWS.HR_LOCATION = Location;

                            ctx.Entry(original).CurrentValues.SetValues(modifiedEmployeeEIMWS);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("Employee with EMP_NUMBER:{0}  was not found.", modifiedEmployeeEIMWS.EMP_NUMBER)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateEmployeeEIMWS : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Delete Employee EIM Work Station Details record
        /// </summary>
        /// <param name="deletingEmployeeEIMWS">The deleting employee eimws.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteEmployeeEIMWS(HR_EMPLOYEE_EIM_WS deletingEmployeeEIMWS)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        HR_EMPLOYEE_EIM_WS original = ctx.HR_EMPLOYEE_EIM_WS.Find(deletingEmployeeEIMWS.EMP_NUMBER);

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
                                    string.Format("Employee with EMP_NUMBER:{0}  was not found.", deletingEmployeeEIMWS.EMP_NUMBER)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteEmployeeEIMWS) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Employees the eimws exists.
        /// </summary>
        /// <param name="existsEmployeeEIMWS">The exists employee eimws.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool EmployeeEIMWSExists(HR_EMPLOYEE_EIM_WS existsEmployeeEIMWS)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    HR_EMPLOYEE_EIM_WS original = ctx.HR_EMPLOYEE_EIM_WS.Find(existsEmployeeEIMWS.EMP_NUMBER);

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
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred EmployeeEIMWS : {0} ", ex.Message));
                return false;
            }
        }
        /// <summary>
        /// Queries the employee eimws.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;HR_EMPLOYEE_EIM_WS&gt;.</returns>
        public ResultDTO<HR_EMPLOYEE_EIM_WS> QueryEmployeeEIMWS(EmployeeEIMWSQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.HR_EMPLOYEE_EIM_WS.AsQueryable();

                    if (query.EmployeeNumber != null)
                    {
                        c = c.Where(i => i.EMP_NUMBER == query.EmployeeNumber);
                    }
                    if (query.Payroll != null)
                    {
                        c = c.Where(i => i.EMP_PAYROLLNO == query.Payroll);
                    }
                    if (query.Location != null)
                    {
                        c = c.Where(i => i.LOC_CODE == query.Location);
                    }
                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.EMP_NUMBER).Skip((pageNumber - 1) * pageSize).Take(pageSize).Include(s => s.HR_EMPLOYEE).Include(s => s.HR_LOCATION).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<HR_EMPLOYEE_EIM_WS>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on QueryEmployeeEIMWS : {0} ", ex.Message));
                return null;
            }
        }

        #endregion

        #region Nationality
        /// <summary>
        /// Queries the nationality.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;HR_NATIONALITY&gt;.</returns>
        public ResultDTO<HR_NATIONALITY> QueryNationality(NationalityQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.HR_NATIONALITY.AsQueryable();

                    if (query.Code != null)
                    {
                        c = c.Where(i => i.NAT_CODE == query.Code);
                    }
                    if (query.Name != null)
                    {
                        c = c.Where(i => i.NAT_NAME == query.Name);
                    }
                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.NAT_CODE).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<HR_NATIONALITY>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on QueryNationality : {0} ", ex.Message));
                return null;
            }
        }
        #endregion

        #region Riligion
        /// <summary>
        /// Queries the religion.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;HR_RELIGION&gt;.</returns>
        public ResultDTO<HR_RELIGION> QueryReligion(ReligionQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.HR_RELIGION.AsQueryable();

                    if (query.Code != null)
                    {
                        c = c.Where(i => i.RLG_CODE == query.Code);
                    }
                    if (query.Name != null)
                    {
                        c = c.Where(i => i.RLG_NAME == query.Name);
                    }
                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.RLG_CODE).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<HR_RELIGION>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on QueryReligion : {0} ", ex.Message));
                return null;
            }
        }
        #endregion

        #region Salary Grade
        /// <summary>
        /// Queries the salary grade.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;HR_SALARY_GRADE&gt;.</returns>
        public ResultDTO<HR_SALARY_GRADE> QuerySalaryGrade(SalaryGradeQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.HR_SALARY_GRADE.AsQueryable();

                    if (query.Code != null)
                    {
                        c = c.Where(i => i.SAL_GRD_CODE == query.Code);
                    }
                    if (query.Name != null)
                    {
                        c = c.Where(i => i.SAL_GRD_NAME == query.Name);
                    }
                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.SAL_GRD_CODE).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<HR_SALARY_GRADE>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on QuerySalaryGrade : {0} ", ex.Message));
                return null;
            }
        }
        #endregion

        #region Corporate Title
        /// <summary>
        /// Queries the corporate title.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;HR_CORPORATE_TITLE&gt;.</returns>
        public ResultDTO<HR_CORPORATE_TITLE> QueryCorporateTitle(CorporateTitleQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.HR_CORPORATE_TITLE.AsQueryable();

                    if (query.Code != null)
                    {
                        c = c.Where(i => i.CT_CODE == query.Code);
                    }
                    if (query.Name != null)
                    {
                        c = c.Where(i => i.CT_NAME == query.Name);
                    }
                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.CT_CODE).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<HR_CORPORATE_TITLE>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on QueryCorporateTitle : {0} ", ex.Message));
                return null;
            }
        }

        #endregion

        #region Designation
        /// <summary>
        /// Queries the designation.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;HR_DESIGNATION&gt;.</returns>
        public ResultDTO<HR_DESIGNATION> QueryDesignation(DesignationQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.HR_DESIGNATION.AsQueryable();

                    if (query.Code != null)
                    {
                        c = c.Where(i => i.DSG_CODE == query.Code);
                    }
                    if (query.Name != null)
                    {
                        c = c.Where(i => i.DSG_NAME == query.Name);
                    }
                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.DSG_CODE).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<HR_DESIGNATION>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on QueryDesignation : {0} ", ex.Message));
                return null;
            }
        }
        #endregion

        #region JDCategory
        /// <summary>
        /// Queries the jd category.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;HR_JD_CATEGORY&gt;.</returns>
        public ResultDTO<HR_JD_CATEGORY> QueryJDCategory(JDCategoryQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.HR_JD_CATEGORY.AsQueryable();

                    if (query.Code != null)
                    {
                        c = c.Where(i => i.JDCAT_CODE == query.Code);
                    }
                    if (query.Name != null)
                    {
                        c = c.Where(i => i.JDCAT_NAME == query.Name);
                    }
                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.JDCAT_CODE).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<HR_JD_CATEGORY>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on QueryJDCategory : {0} ", ex.Message));
                return null;
            }
        }
        #endregion

        #region EMPATTACHMENT

        /// <summary>
        /// Creates the empattachment.
        /// </summary>
        /// <param name="new_HR_EMP_ATTACHMENT">The new_ h r_ em p_ attachment.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateEMPATTACHMENT(HR_EMP_ATTACHMENT new_HR_EMP_ATTACHMENT)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        ctx.HR_EMP_ATTACHMENT.Add(new_HR_EMP_ATTACHMENT);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new EMPATTACHMENT : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Reads the empattachmen tby key.
        /// </summary>
        /// <param name="EMP_NUMBER">The em p_ number.</param>
        /// <param name="EATTACH_ID">The eattac h_ identifier.</param>
        /// <returns>HR_EMP_ATTACHMENT.</returns>
        public HR_EMP_ATTACHMENT ReadEMPATTACHMENTbyKey(string EMP_NUMBER, decimal EATTACH_ID)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMP_ATTACHMENT.Where(c => c.EMP_NUMBER == EMP_NUMBER && c.EATTACH_ID == EATTACH_ID).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadEMPATTACHMENTbyKey : {0} ", ex.Message));
                return null;
            }
        }


        /// <summary>
        /// Return all EMPATTACHMENT records
        /// </summary>
        /// <returns>HR_EMP_ATTACHMENT[].</returns>
        public HR_EMP_ATTACHMENT[] ReadAllEMPATTACHMENT()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMP_ATTACHMENT.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAllEMPATTACHMENT : {0} ", ex.Message));
                return new HR_EMP_ATTACHMENT[0];
            }
        }

        /// <summary>
        /// Execute the query and Return EMPATTACHMENT records
        /// </summary>
        /// <returns>IQueryable&lt;HR_EMP_ATTACHMENT&gt;.</returns>
        public IQueryable<HR_EMP_ATTACHMENT> ReadEMPATTACHMENT()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMP_ATTACHMENT.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadEMPATTACHMENT : {0} ", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Update EMPATTACHMENT record
        /// </summary>
        /// <param name="modifiedEMPATTACHMENT">The modified empattachment.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateEMPATTACHMENT(HR_EMP_ATTACHMENT modifiedEMPATTACHMENT)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        HR_EMP_ATTACHMENT original = ctx.HR_EMP_ATTACHMENT.Find(modifiedEMPATTACHMENT.EMP_NUMBER, modifiedEMPATTACHMENT.EATTACH_ID);

                        if (original != null)
                        {
                            ctx.Entry(original).CurrentValues.SetValues(modifiedEMPATTACHMENT);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("EMPATTACHMENT with EMP_NUMBER:{0} EATTACH_ID:{1}  was not found.", modifiedEMPATTACHMENT.EMP_NUMBER, modifiedEMPATTACHMENT.EATTACH_ID)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateEMPATTACHMENT : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Delete EMPATTACHMENT record
        /// </summary>
        /// <param name="deletingEMPATTACHMENT">The deleting empattachment.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteEMPATTACHMENT(HR_EMP_ATTACHMENT deletingEMPATTACHMENT)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        HR_EMP_ATTACHMENT original = ctx.HR_EMP_ATTACHMENT.Find(deletingEMPATTACHMENT.EMP_NUMBER, deletingEMPATTACHMENT.EATTACH_ID);

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
                                    string.Format("EMPATTACHMENT with EMP_NUMBER:{0} EATTACH_ID:{1}  was not found.", deletingEMPATTACHMENT.EMP_NUMBER, deletingEMPATTACHMENT.EATTACH_ID)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteEMPATTACHMENT) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Empattachments the exists.
        /// </summary>
        /// <param name="existsEMPATTACHMENT">The exists empattachment.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool EMPATTACHMENTExists(HR_EMP_ATTACHMENT existsEMPATTACHMENT)
        {
            try
            {

                using (var ctx = new MDMEntities())
                {
                    HR_EMP_ATTACHMENT original = ctx.HR_EMP_ATTACHMENT.Find(existsEMPATTACHMENT.EMP_NUMBER, existsEMPATTACHMENT.EATTACH_ID);

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
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred EMPATTACHMENT : {0} ", ex.Message));
                return false;
            }
        }
        #endregion

        #region EMPBANK

        /// <summary>
        /// Creates the empbank.
        /// </summary>
        /// <param name="new_HR_EMP_BANK">The new_ h r_ em p_ bank.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateEMPBANK(HR_EMP_BANK new_HR_EMP_BANK)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        ctx.HR_EMP_BANK.Add(new_HR_EMP_BANK);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new EMPBANK : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Reads the empban kby key.
        /// </summary>
        /// <param name="EMP_NUMBER">The em p_ number.</param>
        /// <param name="BBRANCH_CODE">The bbranc h_ code.</param>
        /// <param name="BANK_CODE">The ban k_ code.</param>
        /// <returns>HR_EMP_BANK.</returns>
        public HR_EMP_BANK ReadEMPBANKbyKey(string EMP_NUMBER, string BBRANCH_CODE, string BANK_CODE)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMP_BANK.Where(c => c.EMP_NUMBER == EMP_NUMBER && c.BBRANCH_CODE == BBRANCH_CODE && c.BANK_CODE == BANK_CODE).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadEMPBANKbyKey : {0} ", ex.Message));
                return null;
            }
        }


        /// <summary>
        /// Return all EMPBANK records
        /// </summary>
        /// <returns>HR_EMP_BANK[].</returns>
        public HR_EMP_BANK[] ReadAllEMPBANK()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMP_BANK.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAllEMPBANK : {0} ", ex.Message));
                return new HR_EMP_BANK[0];
            }
        }

        /// <summary>
        /// Execute the query and Return EMPBANK records
        /// </summary>
        /// <returns>IQueryable&lt;HR_EMP_BANK&gt;.</returns>
        public IQueryable<HR_EMP_BANK> ReadEMPBANK()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMP_BANK.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadEMPBANK : {0} ", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Update EMPBANK record
        /// </summary>
        /// <param name="modifiedEMPBANK">The modified empbank.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateEMPBANK(HR_EMP_BANK modifiedEMPBANK)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        HR_EMP_BANK original = ctx.HR_EMP_BANK.Find(modifiedEMPBANK.EMP_NUMBER, modifiedEMPBANK.BBRANCH_CODE, modifiedEMPBANK.BANK_CODE);

                        if (original != null)
                        {
                            ctx.Entry(original).CurrentValues.SetValues(modifiedEMPBANK);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("EMPBANK with EMP_NUMBER:{0} BBRANCH_CODE:{1} BANK_CODE:{2}  was not found.", modifiedEMPBANK.EMP_NUMBER, modifiedEMPBANK.BBRANCH_CODE, modifiedEMPBANK.BANK_CODE)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateEMPBANK : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Delete EMPBANK record
        /// </summary>
        /// <param name="deletingEMPBANK">The deleting empbank.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteEMPBANK(HR_EMP_BANK deletingEMPBANK)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        HR_EMP_BANK original = ctx.HR_EMP_BANK.Find(deletingEMPBANK.EMP_NUMBER, deletingEMPBANK.BBRANCH_CODE, deletingEMPBANK.BANK_CODE);

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
                                    string.Format("EMPBANK with EMP_NUMBER:{0} BBRANCH_CODE:{1} BANK_CODE:{2}  was not found.", deletingEMPBANK.EMP_NUMBER, deletingEMPBANK.BBRANCH_CODE, deletingEMPBANK.BANK_CODE)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteEMPBANK) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Empbanks the exists.
        /// </summary>
        /// <param name="existsEMPBANK">The exists empbank.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool EMPBANKExists(HR_EMP_BANK existsEMPBANK)
        {
            try
            {

                using (var ctx = new MDMEntities())
                {
                    HR_EMP_BANK original = ctx.HR_EMP_BANK.Find(existsEMPBANK.EMP_NUMBER, existsEMPBANK.BBRANCH_CODE, existsEMPBANK.BANK_CODE);

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
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred EMPBANK : {0} ", ex.Message));
                return false;
            }
        }
        #endregion

        #region EMPPASSPORT

        /// <summary>
        /// Creates the emppassport.
        /// </summary>
        /// <param name="new_HR_EMP_PASSPORT">The new_ h r_ em p_ passport.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateEMPPASSPORT(HR_EMP_PASSPORT new_HR_EMP_PASSPORT)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        ctx.HR_EMP_PASSPORT.Add(new_HR_EMP_PASSPORT);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new EMPPASSPORT : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Reads the emppasspor tby key.
        /// </summary>
        /// <param name="EMP_NUMBER">The em p_ number.</param>
        /// <param name="EP_SEQNO">The e p_ seqno.</param>
        /// <returns>HR_EMP_PASSPORT.</returns>
        public HR_EMP_PASSPORT ReadEMPPASSPORTbyKey(string EMP_NUMBER, decimal EP_SEQNO)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMP_PASSPORT.Where(c => c.EMP_NUMBER == EMP_NUMBER && c.EP_SEQNO == EP_SEQNO).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadEMPPASSPORTbyKey : {0} ", ex.Message));
                return null;
            }
        }


        /// <summary>
        /// Return all EMPPASSPORT records
        /// </summary>
        /// <returns>HR_EMP_PASSPORT[].</returns>
        public HR_EMP_PASSPORT[] ReadAllEMPPASSPORT()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMP_PASSPORT.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAllEMPPASSPORT : {0} ", ex.Message));
                return new HR_EMP_PASSPORT[0];
            }
        }

        /// <summary>
        /// Execute the query and Return EMPPASSPORT records
        /// </summary>
        /// <returns>IQueryable&lt;HR_EMP_PASSPORT&gt;.</returns>
        public IQueryable<HR_EMP_PASSPORT> ReadEMPPASSPORT()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMP_PASSPORT.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadEMPPASSPORT : {0} ", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Update EMPPASSPORT record
        /// </summary>
        /// <param name="modifiedEMPPASSPORT">The modified emppassport.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateEMPPASSPORT(HR_EMP_PASSPORT modifiedEMPPASSPORT)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        HR_EMP_PASSPORT original = ctx.HR_EMP_PASSPORT.Find(modifiedEMPPASSPORT.EMP_NUMBER, modifiedEMPPASSPORT.EP_SEQNO);

                        if (original != null)
                        {
                            ctx.Entry(original).CurrentValues.SetValues(modifiedEMPPASSPORT);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("EMPPASSPORT with EMP_NUMBER:{0} EP_SEQNO:{1}  was not found.", modifiedEMPPASSPORT.EMP_NUMBER, modifiedEMPPASSPORT.EP_SEQNO)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateEMPPASSPORT : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Delete EMPPASSPORT record
        /// </summary>
        /// <param name="deletingEMPPASSPORT">The deleting emppassport.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteEMPPASSPORT(HR_EMP_PASSPORT deletingEMPPASSPORT)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        HR_EMP_PASSPORT original = ctx.HR_EMP_PASSPORT.Find(deletingEMPPASSPORT.EMP_NUMBER, deletingEMPPASSPORT.EP_SEQNO);

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
                                    string.Format("EMPPASSPORT with EMP_NUMBER:{0} EP_SEQNO:{1}  was not found.", deletingEMPPASSPORT.EMP_NUMBER, deletingEMPPASSPORT.EP_SEQNO)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteEMPPASSPORT) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Emppassports the exists.
        /// </summary>
        /// <param name="existsEMPPASSPORT">The exists emppassport.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool EMPPASSPORTExists(HR_EMP_PASSPORT existsEMPPASSPORT)
        {
            try
            {

                using (var ctx = new MDMEntities())
                {
                    HR_EMP_PASSPORT original = ctx.HR_EMP_PASSPORT.Find(existsEMPPASSPORT.EMP_NUMBER, existsEMPPASSPORT.EP_SEQNO);

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
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred EMPPASSPORT : {0} ", ex.Message));
                return false;
            }
        }
        #endregion

        # region HR Dependents
        /// <summary>
        /// Add a HR Dependents
        /// </summary>
        /// <param name="new_HR_Dependent">The new_ h r_ dependent.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateHRDependent(HR_EMP_RELATIONINFO new_HR_Dependent)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        ctx.HR_EMP_RELATIONINFO.Add(new_HR_Dependent);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };

            }

            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new HRDependent : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Queries the hr dependent.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>IEnumerable&lt;HR_EMP_RELATIONINFO&gt;.</returns>
        public IEnumerable<HR_EMP_RELATIONINFO> QueryHRDependent(HRDependentQuery query)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.HR_EMP_RELATIONINFO.AsEnumerable();

                    if (query.EmpNumber != null)
                    {
                        c = c.Where(i => i.EMP_NUMBER == query.EmpNumber);
                    }
                    if (query.DependentID != null)
                    {
                        c = c.Where(i => i.EREL_DEPENDNO == query.DependentID);
                    }

                    return c.ToList();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on QueryHRDependent : {0} ", ex.Message));
                return null;
            }
        }
        /// <summary>
        /// Hrs the dependent exists.
        /// </summary>
        /// <param name="findHRDependent">The find hr dependent.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool HRDependentExists(HR_EMP_RELATIONINFO findHRDependent)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    HR_EMP_RELATIONINFO record = ctx.HR_EMP_RELATIONINFO.Find(findHRDependent.EMP_NUMBER, findHRDependent.EREL_DEPENDNO);
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
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on HRDependentExists : {0} ", ex.Message));
                return false;
            }
        }
        /// <summary>
        /// Reads the hr dependentby key.
        /// </summary>
        /// <param name="EmpID">The emp identifier.</param>
        /// <param name="DependentID">The dependent identifier.</param>
        /// <returns>HR_EMP_RELATIONINFO.</returns>
        public HR_EMP_RELATIONINFO ReadHRDependentbyKey(string EmpID, decimal DependentID)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMP_RELATIONINFO.Where(c => c.EMP_NUMBER == EmpID && c.EREL_DEPENDNO == DependentID).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadHRDependentbyKey : {0} ", ex.Message));
                return null;
            }
        }


        /// <summary>
        /// Return all HR Dependent records
        /// </summary>
        /// <returns>HR_EMP_RELATIONINFO[].</returns>
        public HR_EMP_RELATIONINFO[] ReadAllHRDependent()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMP_RELATIONINFO.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAllHRDependent : {0} ", ex.Message));
                return new HR_EMP_RELATIONINFO[0];
            }
        }



        /// <summary>
        /// Update HR Dependent record
        /// </summary>
        /// <param name="modifiedHRDependent">The modified hr dependent.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateHRDependent(HR_EMP_RELATIONINFO modifiedHRDependent)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        HR_EMP_RELATIONINFO original = ctx.HR_EMP_RELATIONINFO.Find(modifiedHRDependent.EMP_NUMBER, modifiedHRDependent.EREL_DEPENDNO);

                        if (original != null)
                        {
                            ctx.Entry(original).CurrentValues.SetValues(modifiedHRDependent);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("HR Attachment with EmpID:{0} Attachment ID:{1} was not found.", modifiedHRDependent.EMP_NUMBER, modifiedHRDependent.EREL_DEPENDNO)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateHRAttachment : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Delete HR Dependent record
        /// </summary>
        /// <param name="deletingHRDependent">The deleting hr dependent.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteHRDependent(HR_EMP_RELATIONINFO deletingHRDependent)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        HR_EMP_RELATIONINFO original = ctx.HR_EMP_RELATIONINFO.Find(deletingHRDependent.EMP_NUMBER, deletingHRDependent.EREL_DEPENDNO);

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
                                    string.Format("HR Attachment with Emp ID:{0} Dependent ID:{1}  was not found.", deletingHRDependent.EMP_NUMBER, deletingHRDependent.EREL_DEPENDNO)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteHRDependent) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        #endregion

        #region EMPQUALIFICATION

        /// <summary>
        /// Queries the empqualification.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;HR_EMP_QUALIFICATION&gt;.</returns>
        public ResultDTO<HR_EMP_QUALIFICATION> QueryEMPQUALIFICATION(HRQualificationQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {

                    var c = ctx.HR_EMP_QUALIFICATION.AsQueryable();

                    if (query.EmployeeNumber != null)
                    {
                        c = c.Where(i => i.EMP_NUMBER == query.EmployeeNumber);
                    }
                    if (query.QualificationCode != null)
                    {
                        c = c.Where(i => i.QUALIFI_CODE == query.QualificationCode);
                    }
                    if (query.QualificationYear != null)
                    {
                        c = c.Where(i => i.EQUALIFI_YEAR == query.QualificationYear);
                    }
                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.EMP_NUMBER).Skip((pageNumber - 1) * pageSize).Take(pageSize).Include(s => s.HR_EMPLOYEE).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<HR_EMP_QUALIFICATION>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
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
        /// Creates the empqualification.
        /// </summary>
        /// <param name="new_HR_EMP_QUALIFICATION">The new_ h r_ em p_ qualification.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateEMPQUALIFICATION(HR_EMP_QUALIFICATION new_HR_EMP_QUALIFICATION)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        new_HR_EMP_QUALIFICATION.HR_EMPLOYEE = ctx.HR_EMPLOYEE.Find(new_HR_EMP_QUALIFICATION.HR_EMPLOYEE.EMP_NUMBER);
                        ctx.HR_EMP_QUALIFICATION.Add(new_HR_EMP_QUALIFICATION);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new EMPQUALIFICATION : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Reads the empqualificatio nby key.
        /// </summary>
        /// <param name="EMP_NUMBER">The em p_ number.</param>
        /// <param name="QUALIFI_CODE">The qualif i_ code.</param>
        /// <returns>HR_EMP_QUALIFICATION.</returns>
        public HR_EMP_QUALIFICATION ReadEMPQUALIFICATIONbyKey(string EMP_NUMBER, string QUALIFI_CODE)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMP_QUALIFICATION.Where(c => c.EMP_NUMBER == EMP_NUMBER && c.QUALIFI_CODE == QUALIFI_CODE).Include(s => s.HR_EMPLOYEE).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadEMPQUALIFICATIONbyKey : {0} ", ex.Message));
                return null;
            }
        }


        /// <summary>
        /// Return all EMPQUALIFICATION records
        /// </summary>
        /// <returns>HR_EMP_QUALIFICATION[].</returns>
        public HR_EMP_QUALIFICATION[] ReadAllEMPQUALIFICATION()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMP_QUALIFICATION.Include(s => s.HR_EMPLOYEE).ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAllEMPQUALIFICATION : {0} ", ex.Message));
                return new HR_EMP_QUALIFICATION[0];
            }
        }

        /// <summary>
        /// Execute the query and Return EMPQUALIFICATION records
        /// </summary>
        /// <returns>IQueryable&lt;HR_EMP_QUALIFICATION&gt;.</returns>
        public IQueryable<HR_EMP_QUALIFICATION> ReadEMPQUALIFICATION()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMP_QUALIFICATION.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadEMPQUALIFICATION : {0} ", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Update EMPQUALIFICATION record
        /// </summary>
        /// <param name="modifiedEMPQUALIFICATION">The modified empqualification.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateEMPQUALIFICATION(HR_EMP_QUALIFICATION modifiedEMPQUALIFICATION)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        HR_EMP_QUALIFICATION original = ctx.HR_EMP_QUALIFICATION.Find(modifiedEMPQUALIFICATION.HR_EMPLOYEE.EMP_NUMBER, modifiedEMPQUALIFICATION.QUALIFI_CODE);

                        if (original != null)
                        {
                            ctx.Entry(original).CurrentValues.SetValues(modifiedEMPQUALIFICATION);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("EMPQUALIFICATION with EMP_NUMBER:{0} QUALIFI_CODE:{1}  was not found.", modifiedEMPQUALIFICATION.EMP_NUMBER, modifiedEMPQUALIFICATION.QUALIFI_CODE)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateEMPQUALIFICATION : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Delete EMPQUALIFICATION record
        /// </summary>
        /// <param name="deletingEMPQUALIFICATION">The deleting empqualification.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteEMPQUALIFICATION(HR_EMP_QUALIFICATION deletingEMPQUALIFICATION)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        HR_EMP_QUALIFICATION original = ctx.HR_EMP_QUALIFICATION.Find(deletingEMPQUALIFICATION.HR_EMPLOYEE.EMP_NUMBER, deletingEMPQUALIFICATION.QUALIFI_CODE);

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
                                    string.Format("EMPQUALIFICATION with EMP_NUMBER:{0} QUALIFI_CODE:{1}  was not found.", deletingEMPQUALIFICATION.EMP_NUMBER, deletingEMPQUALIFICATION.QUALIFI_CODE)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteEMPQUALIFICATION) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Empqualifications the exists.
        /// </summary>
        /// <param name="existsEMPQUALIFICATION">The exists empqualification.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool EMPQUALIFICATIONExists(HR_EMP_QUALIFICATION existsEMPQUALIFICATION)
        {
            try
            {

                using (var ctx = new MDMEntities())
                {
                    HR_EMP_QUALIFICATION original = ctx.HR_EMP_QUALIFICATION.Find(existsEMPQUALIFICATION.HR_EMPLOYEE.EMP_NUMBER, existsEMPQUALIFICATION.QUALIFI_CODE);

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
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred EMPQUALIFICATION : {0} ", ex.Message));
                return false;
            }
        }
        #endregion

        #region EMPWORKEXPERIENCE

        /// <summary>
        /// Queries the empworkexperience.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;HR_EMP_WORK_EXPERIENCE&gt;.</returns>
        public ResultDTO<HR_EMP_WORK_EXPERIENCE> QueryEMPWORKEXPERIENCE(HRWorkExperienceQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {

                    var c = ctx.HR_EMP_WORK_EXPERIENCE.AsQueryable();

                    if (query.EmployeeNumber != null)
                    {
                        c = c.Where(i => i.EMP_NUMBER == query.EmployeeNumber);
                    }
                    if (query.Company != null)
                    {
                        c = c.Where(i => i.EEXP_COMPANY == query.Company);
                    }
                    if (query.Years != null)
                    {
                        c = c.Where(i => i.EEXP_YEARS == query.Years);
                    }
                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.EMP_NUMBER).Skip((pageNumber - 1) * pageSize).Take(pageSize).Include(s => s.HR_EMPLOYEE).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<HR_EMP_WORK_EXPERIENCE>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
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
        /// Creates the empwork.
        /// </summary>
        /// <param name="new_HR_EMP_WORK_EXPERIENCE">The new_ h r_ em p_ wor k_ experience.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateEMPWORK(HR_EMP_WORK_EXPERIENCE new_HR_EMP_WORK_EXPERIENCE)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        new_HR_EMP_WORK_EXPERIENCE.HR_EMPLOYEE = ctx.HR_EMPLOYEE.Find(new_HR_EMP_WORK_EXPERIENCE.HR_EMPLOYEE.EMP_NUMBER);
                        ctx.HR_EMP_WORK_EXPERIENCE.Add(new_HR_EMP_WORK_EXPERIENCE);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new EMPWORK : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Reads the empwor kby key.
        /// </summary>
        /// <param name="EMP_NUMBER">The em p_ number.</param>
        /// <returns>HR_EMP_WORK_EXPERIENCE.</returns>
        public HR_EMP_WORK_EXPERIENCE ReadEMPWORKbyKey(string EMP_NUMBER)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMP_WORK_EXPERIENCE.Where(c => c.EMP_NUMBER == EMP_NUMBER).Include(s => s.HR_EMPLOYEE).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadEMPWORKbyKey : {0} ", ex.Message));
                return null;
            }
        }


        /// <summary>
        /// Return all EMPWORK records
        /// </summary>
        /// <returns>HR_EMP_WORK_EXPERIENCE[].</returns>
        public HR_EMP_WORK_EXPERIENCE[] ReadAllEMPWORK()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMP_WORK_EXPERIENCE.Include(s => s.HR_EMPLOYEE).ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadAllEMPWORK : {0} ", ex.Message));
                return new HR_EMP_WORK_EXPERIENCE[0];
            }
        }

        /// <summary>
        /// Execute the query and Return EMPWORK records
        /// </summary>
        /// <returns>IQueryable&lt;HR_EMP_WORK_EXPERIENCE&gt;.</returns>
        public IQueryable<HR_EMP_WORK_EXPERIENCE> ReadEMPWORK()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMP_WORK_EXPERIENCE.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadEMPWORK : {0} ", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Update EMPWORK record
        /// </summary>
        /// <param name="modifiedEMPWORK">The modified empwork.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateEMPWORK(HR_EMP_WORK_EXPERIENCE modifiedEMPWORK)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        HR_EMP_WORK_EXPERIENCE original = ctx.HR_EMP_WORK_EXPERIENCE.Find(modifiedEMPWORK.HR_EMPLOYEE.EMP_NUMBER);

                        if (original != null)
                        {
                            ctx.Entry(original).CurrentValues.SetValues(modifiedEMPWORK);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("EMPWORK with EMP_NUMBER:{0}  was not found.", modifiedEMPWORK.EMP_NUMBER)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateEMPWORK : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Delete EMPWORK record
        /// </summary>
        /// <param name="deletingEMPWORK">The deleting empwork.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteEMPWORK(HR_EMP_WORK_EXPERIENCE deletingEMPWORK)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        HR_EMP_WORK_EXPERIENCE original = ctx.HR_EMP_WORK_EXPERIENCE.Find(deletingEMPWORK.HR_EMPLOYEE.EMP_NUMBER);

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
                                    string.Format("EMPWORK with EMP_NUMBER:{0}  was not found.", deletingEMPWORK.EMP_NUMBER)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteEMPWORK) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Empworks the exists.
        /// </summary>
        /// <param name="existsEMPWORK">The exists empwork.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool EMPWORKExists(HR_EMP_WORK_EXPERIENCE existsEMPWORK)
        {
            try
            {

                using (var ctx = new MDMEntities())
                {
                    HR_EMP_WORK_EXPERIENCE original = ctx.HR_EMP_WORK_EXPERIENCE.Find(existsEMPWORK.HR_EMPLOYEE.EMP_NUMBER);

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
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred EMPWORK : {0} ", ex.Message));
                return false;
            }
        }
        #endregion 

        #region Emergency

        /// <summary>
        /// Execute the query and return Emergency records
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;HR_EMP_EMERGENCY&gt;.</returns>
        public ResultDTO<HR_EMP_EMERGENCY> QueryEmergency(EmergencyQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.HR_EMP_EMERGENCY.AsQueryable();
                    if (query.EmployeeNumber != null)
                    {
                        c = c.Where(i => i.EMP_NUMBER == query.EmployeeNumber);
                    }

                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.EMP_NUMBER).Skip((pageNumber - 1) * pageSize).Include(e => e.HR_EMPLOYEE).Take(pageSize).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<HR_EMP_EMERGENCY>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
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
        /// Create a new Emergency record
        /// </summary>
        /// <param name="new_Emergency">The new_ emergency.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateEmergency(HR_EMP_EMERGENCY new_Emergency)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        var emp_number = ctx.HR_EMPLOYEE.Find(new_Emergency.HR_EMPLOYEE.EMP_NUMBER);
                        new_Emergency.HR_EMPLOYEE = emp_number;
                        ctx.HR_EMP_EMERGENCY.Add(new_Emergency);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new Emergency : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Return a Emergency record
        /// </summary>
        /// <param name="emp_number">The emp_number.</param>
        /// <returns>HR_EMP_EMERGENCY.</returns>
        public HR_EMP_EMERGENCY ReadEmergencyByKey(string emp_number)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMP_EMERGENCY.Where(c => c.EMP_NUMBER == emp_number).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on read Emergency by key : {0} ", ex.Message));
                return null;
            }
        }


        /// <summary>
        /// Return all Emergency records
        /// </summary>
        /// <returns>HR_EMP_EMERGENCY[].</returns>
        public HR_EMP_EMERGENCY[] ReadAllEmergency()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMP_EMERGENCY.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on read all Emergency : {0} ", ex.Message));
                return new HR_EMP_EMERGENCY[0];
            }
        }


        /// <summary>
        /// Update Emergency record
        /// </summary>
        /// <param name="modified_Emergency">The modified_ emergency.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateEmergency(HR_EMP_EMERGENCY modified_Emergency)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        HR_EMP_EMERGENCY original_Emergency = ctx.HR_EMP_EMERGENCY.Find(modified_Emergency.EMP_NUMBER);

                        if (original_Emergency != null)
                        {
                            ctx.Entry(original_Emergency).CurrentValues.SetValues(modified_Emergency);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage = string.Format("Emergency with employee number:{0} was not found.", modified_Emergency.EMP_NUMBER)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on update Emergency: {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Delete Emergency record
        /// </summary>
        /// <param name="delete_Emergency">The delete_ emergency.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteEmergency(HR_EMP_EMERGENCY delete_Emergency)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        HR_EMP_EMERGENCY original_Emergency = ctx.HR_EMP_EMERGENCY.Find(delete_Emergency.EMP_NUMBER);

                        if (original_Emergency != null)
                        {
                            ctx.HR_EMP_EMERGENCY.Remove(original_Emergency);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage = string.Format("Emergency with employee number:{0} was not found.", delete_Emergency.EMP_NUMBER)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on delete Emergency : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Exists the emergency.
        /// </summary>
        /// <param name="exist_Emergency">The exist_ emergency.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool ExistEmergency(HR_EMP_EMERGENCY exist_Emergency)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    HR_EMP_EMERGENCY original_Emergency = ctx.HR_EMP_EMERGENCY.Find(exist_Emergency.HR_EMPLOYEE.EMP_NUMBER);

                    if (original_Emergency != null)
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
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred Emergency : {0} ", ex.Message));
                return false;
            }
        }
        #endregion

        #region Transport

        /// <summary>
        /// Execute the query and return Transport records
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;HR_EMP_TRANSPORT&gt;.</returns>
        public ResultDTO<HR_EMP_TRANSPORT> QueryTransport(TransportQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.HR_EMP_TRANSPORT.AsQueryable();
                    if (query.EmployeeNumber != null)
                    {
                        c = c.Where(i => i.EMP_NUMBER == query.EmployeeNumber);
                    }

                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.EMP_NUMBER).Skip((pageNumber - 1) * pageSize).Include(s => s.HR_EMPLOYEE).Take(pageSize).ToArray();//need tix
                    int localcount = result.Count();
                    var dto = new ResultDTO<HR_EMP_TRANSPORT>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
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
        /// Create a new Transport record
        /// </summary>
        /// <param name="new_Transport">The new_ transport.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateTransport(HR_EMP_TRANSPORT new_Transport)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        var emp_number = ctx.HR_EMPLOYEE.Find(new_Transport.HR_EMPLOYEE.EMP_NUMBER);
                        new_Transport.HR_EMPLOYEE = emp_number;
                        ctx.HR_EMP_TRANSPORT.Add(new_Transport);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new Transport : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Return a Transport record
        /// </summary>
        /// <param name="emp_number">The emp_number.</param>
        /// <returns>HR_EMP_TRANSPORT.</returns>
        public HR_EMP_TRANSPORT ReadTransportByKey(string emp_number)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMP_TRANSPORT.Where(c => c.EMP_NUMBER == emp_number).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on read Transport by key : {0} ", ex.Message));
                return null;
            }
        }


        /// <summary>
        /// Return all Transport records
        /// </summary>
        /// <returns>HR_EMP_TRANSPORT[].</returns>
        public HR_EMP_TRANSPORT[] ReadAllTransport()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMP_TRANSPORT.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on read all Transport : {0} ", ex.Message));
                return new HR_EMP_TRANSPORT[0];
            }
        }


        /// <summary>
        /// Update Transport record
        /// </summary>
        /// <param name="modified_Transport">The modified_ transport.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateTransport(HR_EMP_TRANSPORT modified_Transport)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        HR_EMP_TRANSPORT original_Transport = ctx.HR_EMP_TRANSPORT.Find(modified_Transport.EMP_NUMBER);

                        if (original_Transport != null)
                        {
                            ctx.Entry(original_Transport).CurrentValues.SetValues(modified_Transport);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage = string.Format("Transport with employee number:{0} was not found.", modified_Transport.EMP_NUMBER)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on update Transport: {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Delete Transport record
        /// </summary>
        /// <param name="delete_Transport">The delete_ transport.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteTransport(HR_EMP_TRANSPORT delete_Transport)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        HR_EMP_TRANSPORT original_Transport = ctx.HR_EMP_TRANSPORT.Find(delete_Transport.EMP_NUMBER);

                        if (original_Transport != null)
                        {
                            ctx.HR_EMP_TRANSPORT.Remove(original_Transport);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage = string.Format("Transport with employee number:{0} was not found.", delete_Transport.EMP_NUMBER)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on delete Transport : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Exists the transport.
        /// </summary>
        /// <param name="exist_Transport">The exist_ transport.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool ExistTransport(HR_EMP_TRANSPORT exist_Transport)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    HR_EMP_TRANSPORT original_Transport = ctx.HR_EMP_TRANSPORT.Find(exist_Transport.HR_EMPLOYEE.EMP_NUMBER);

                    if (original_Transport != null)
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
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred Transport : {0} ", ex.Message));
                return false;
            }
        }
        #endregion

        #region Training

        /// <summary>
        /// Execute the query and return Training records
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;HR_EMP_TRAININGS&gt;.</returns>
        public ResultDTO<HR_EMP_TRAININGS> QueryTraining(TrainingQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.HR_EMP_TRAININGS.AsQueryable();
                    if (query.EmployeeNumber != null)
                    {
                        c = c.Where(i => i.EMP_NUMBER == query.EmployeeNumber);
                    }
                    if (query.TrainingID != null)
                    {
                        c = c.Where(i => i.TN_ID == query.TrainingID);
                    }

                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.EMP_NUMBER).Skip((pageNumber - 1) * pageSize).Include(s => s.HR_EMPLOYEE).Take(pageSize).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<HR_EMP_TRAININGS>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
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
        /// Create a new Training record
        /// </summary>
        /// <param name="new_Training">The new_ training.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateTraining(HR_EMP_TRAININGS new_Training)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        var emp_number = ctx.HR_EMPLOYEE.Find(new_Training.HR_EMPLOYEE.EMP_NUMBER);
                        new_Training.HR_EMPLOYEE = emp_number;
                        ctx.HR_EMP_TRAININGS.Add(new_Training);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new Training : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Return a Training record
        /// </summary>
        /// <param name="emp_number">The emp_number.</param>
        /// <param name="tn_id">The tn_id.</param>
        /// <returns>HR_EMP_TRAININGS.</returns>
        public HR_EMP_TRAININGS ReadTrainingByKey(string emp_number, int tn_id)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMP_TRAININGS.Where(c => c.EMP_NUMBER == emp_number && c.TN_ID == tn_id).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on read Training by key : {0} ", ex.Message));
                return null;
            }
        }


        /// <summary>
        /// Return all Training records
        /// </summary>
        /// <returns>HR_EMP_TRAININGS[].</returns>
        public HR_EMP_TRAININGS[] ReadAllTraining()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMP_TRAININGS.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on read all Training : {0} ", ex.Message));
                return new HR_EMP_TRAININGS[0];
            }
        }


        /// <summary>
        /// Update Training record
        /// </summary>
        /// <param name="modified_Training">The modified_ training.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateTraining(HR_EMP_TRAININGS modified_Training)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        HR_EMP_TRAININGS original_Training = ctx.HR_EMP_TRAININGS.Find(modified_Training.EMP_NUMBER, modified_Training.TN_ID);

                        if (original_Training != null)
                        {
                            ctx.Entry(original_Training).CurrentValues.SetValues(modified_Training);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage = string.Format("Training with employee number:{0} and training ID:{1} was not found.", modified_Training.EMP_NUMBER, modified_Training.TN_ID)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on update Training: {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Delete Training record
        /// </summary>
        /// <param name="delete_Training">The delete_ training.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteTraining(HR_EMP_TRAININGS delete_Training)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        HR_EMP_TRAININGS original_Training = ctx.HR_EMP_TRAININGS.Find(delete_Training.EMP_NUMBER, delete_Training.TN_ID);

                        if (original_Training != null)
                        {
                            ctx.HR_EMP_TRAININGS.Remove(original_Training);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage = string.Format("Training with employee number:{0} and training ID:{1} was not found.", delete_Training.EMP_NUMBER, delete_Training.TN_ID)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on delete Training : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Exists the training.
        /// </summary>
        /// <param name="exist_Training">The exist_ training.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool ExistTraining(HR_EMP_TRAININGS exist_Training)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    HR_EMP_TRAININGS original_Training = ctx.HR_EMP_TRAININGS.Find(exist_Training.HR_EMPLOYEE.EMP_NUMBER, exist_Training.TN_ID);

                    if (original_Training != null)
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
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred Training : {0} ", ex.Message));
                return false;
            }
        }

        #endregion

        #region Warning

        /// <summary>
        /// Execute the query and return warning records
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;HR_EMP_WARNINGS&gt;.</returns>
        public ResultDTO<HR_EMP_WARNINGS> QueryWarning(WarningQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.HR_EMP_WARNINGS.AsQueryable();
                    if (query.EmployeeNumber != null)
                    {
                        c = c.Where(i => i.EMP_NUMBER == query.EmployeeNumber);
                    }
                    if (query.WarningID != null)
                    {
                        c = c.Where(i => i.WRN_ID == query.WarningID);
                    }

                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.EMP_NUMBER).Skip((pageNumber - 1) * pageSize).Include(s => s.HR_EMPLOYEE).Take(pageSize).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<HR_EMP_WARNINGS>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
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
        /// Create a new Warning record
        /// </summary>
        /// <param name="new_Warning">The new_ warning.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateWarning(HR_EMP_WARNINGS new_Warning)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        var emp_number = ctx.HR_EMPLOYEE.Find(new_Warning.HR_EMPLOYEE.EMP_NUMBER);
                        new_Warning.HR_EMPLOYEE = emp_number;
                        ctx.HR_EMP_WARNINGS.Add(new_Warning);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new Warning : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Return a Warning record
        /// </summary>
        /// <param name="emp_number">The emp_number.</param>
        /// <param name="wrn_id">The wrn_id.</param>
        /// <returns>HR_EMP_WARNINGS.</returns>
        public HR_EMP_WARNINGS ReadWarningByKey(string emp_number, int wrn_id)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMP_WARNINGS.Where(c => c.EMP_NUMBER == emp_number && c.WRN_ID == wrn_id).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on read Warning by key : {0} ", ex.Message));
                return null;
            }
        }


        /// <summary>
        /// Return all Warning records
        /// </summary>
        /// <returns>HR_EMP_WARNINGS[].</returns>
        public HR_EMP_WARNINGS[] ReadAllWarning()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMP_WARNINGS.ToArray();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on read all Warning : {0} ", ex.Message));
                return new HR_EMP_WARNINGS[0];
            }
        }


        /// <summary>
        /// Update Warning record
        /// </summary>
        /// <param name="modified_Warning">The modified_ warning.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateWarning(HR_EMP_WARNINGS modified_Warning)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        HR_EMP_WARNINGS original_Warning = ctx.HR_EMP_WARNINGS.Find(modified_Warning.EMP_NUMBER, modified_Warning.WRN_ID);

                        if (original_Warning != null)
                        {
                            ctx.Entry(original_Warning).CurrentValues.SetValues(modified_Warning);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage = string.Format("Training with employee number:{0} and warning ID:{1} was not found.", modified_Warning.EMP_NUMBER, modified_Warning.WRN_ID)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on update Warning: {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Delete Warning record
        /// </summary>
        /// <param name="delete_Warning">The delete_ warning.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteWarning(HR_EMP_WARNINGS delete_Warning)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        HR_EMP_WARNINGS original_Warning = ctx.HR_EMP_WARNINGS.Find(delete_Warning.EMP_NUMBER, delete_Warning.WRN_ID);

                        if (original_Warning != null)
                        {
                            ctx.HR_EMP_WARNINGS.Remove(original_Warning);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage = string.Format("Warning with employee number:{0} and warning ID:{1} was not found.", delete_Warning.EMP_NUMBER, delete_Warning.WRN_ID)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on delete Warning : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Exists the warning.
        /// </summary>
        /// <param name="exist_Warning">The exist_ warning.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool ExistWarning(HR_EMP_WARNINGS exist_Warning)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    HR_EMP_WARNINGS original_Warning = ctx.HR_EMP_WARNINGS.Find(exist_Warning.HR_EMPLOYEE.EMP_NUMBER, exist_Warning.WRN_ID);

                    if (original_Warning != null)
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
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred Warning : {0} ", ex.Message));
                return false;
            }
        }
        #endregion

        #region HR Cash Benefits

        /// <summary>
        /// Queries the hr cash benefits.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;HR_CASH_BENEFIT&gt;.</returns>
        public ResultDTO<HR_CASH_BENEFIT> QueryHRCashBenefits(HRCashBenefitQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.HR_CASH_BENEFIT.AsQueryable();

                    if (query.BenefitCode != null)
                    {
                        c = c.Where(i => i.BEN_CODE == query.BenefitCode);
                    }

                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.BEN_CODE).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<HR_CASH_BENEFIT>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on QueryCashBenifits : {0} ", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Creates the hr cash benefit.
        /// </summary>
        /// <param name="new_HR_Cash_Benifit">The new_ h r_ cash_ benifit.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateHRCashBenefit(HR_CASH_BENEFIT new_HR_Cash_Benifit)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        ctx.HR_CASH_BENEFIT.Add(new_HR_Cash_Benifit);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new HR Cash Benefit : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Reads the hr cash benefitby key.
        /// </summary>
        /// <param name="HRCashBenefit">The hr cash benefit.</param>
        /// <returns>HR_CASH_BENEFIT.</returns>
        public HR_CASH_BENEFIT ReadHRCashBenefitbyKey(string HRCashBenefit)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_CASH_BENEFIT.Where(c => c.BEN_CODE == HRCashBenefit).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadHRCashBenefitbyKey : {0} ", ex.Message));
                return null;
            }
        }


        /// <summary>
        /// Return all HR Cash Beneft records
        /// </summary>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;HR_CASH_BENEFIT&gt;.</returns>
        public ResultDTO<HR_CASH_BENEFIT> ReadAllHRCashBenefit(int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    int totalcount = ctx.HR_CASH_BENEFIT.Count();
                    var result = ctx.HR_CASH_BENEFIT.OrderBy(i => i.BEN_CODE).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<HR_CASH_BENEFIT>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on HR_CASH_BENEFIT : {0} ", ex.Message));
                return new ResultDTO<HR_CASH_BENEFIT>();
            }
        }

        /// <summary>
        /// Execute the query and Return AccountType records
        /// </summary>
        /// <returns>IQueryable&lt;HR_CASH_BENEFIT&gt;.</returns>
        public IQueryable<HR_CASH_BENEFIT> ReadHRCashBenefit()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_CASH_BENEFIT.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadHRCashBenefit : {0} ", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Update AccountType record
        /// </summary>
        /// <param name="modifiedHRCashBenefit">The modified hr cash benefit.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateHRCashBenefit(HR_CASH_BENEFIT modifiedHRCashBenefit)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        HR_CASH_BENEFIT original = ctx.HR_CASH_BENEFIT.Find(modifiedHRCashBenefit.BEN_CODE);

                        if (original != null)
                        {
                            ctx.Entry(original).CurrentValues.SetValues(modifiedHRCashBenefit);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("AccountType with DocCode:{0} was not found.", modifiedHRCashBenefit.BEN_CODE)
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
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new HRCashBenefit : {0} ", errorMsg));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = errorMsg };

            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateHRCashBenefit : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Delete AccountType record
        /// </summary>
        /// <param name="deletingHRCashBenefit">The deleting hr cash benefit.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteHRCashBenefit(HR_CASH_BENEFIT deletingHRCashBenefit)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        HR_CASH_BENEFIT original = ctx.HR_CASH_BENEFIT.Find(deletingHRCashBenefit.BEN_CODE);

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
                                    string.Format("HRCashBenefit with HRCashBenefit:{0}  was not found.", deletingHRCashBenefit.BEN_CODE)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteHRCashBenefit) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Hrs the cash benefit exists.
        /// </summary>
        /// <param name="existsHRCashBenefit">The exists hr cash benefit.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool HRCashBenefitExists(HR_CASH_BENEFIT existsHRCashBenefit)
        {
            try
            {

                using (var ctx = new MDMEntities())
                {
                    HR_CASH_BENEFIT original = ctx.HR_CASH_BENEFIT.Find(existsHRCashBenefit.BEN_CODE);

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
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred HRCashBenefit : {0} ", ex.Message));
                return false;
            }
        }
        #endregion

        #region HR Non Cash Benefits

        /// <summary>
        /// Queries the hr non cash benefits.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;HR_NONCASH_BENEFIT&gt;.</returns>
        public ResultDTO<HR_NONCASH_BENEFIT> QueryHRNonCashBenefits(HRNonCashBenefitQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.HR_NONCASH_BENEFIT.AsQueryable();

                    if (query.NonBenefitCode != null)
                    {
                        c = c.Where(i => i.NBEN_CODE == query.NonBenefitCode);
                    }

                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.NBEN_CODE).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<HR_NONCASH_BENEFIT>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on QueryNonCashBenefits : {0} ", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Creates the hr non cash benefit.
        /// </summary>
        /// <param name="new_HR_Non_Cash_Benifit">The new_ h r_ non_ cash_ benifit.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateHRNonCashBenefit(HR_NONCASH_BENEFIT new_HR_Non_Cash_Benifit)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        ctx.HR_NONCASH_BENEFIT.Add(new_HR_Non_Cash_Benifit);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new HR Non Cash Benefit : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Reads the hr non cash benefitby key.
        /// </summary>
        /// <param name="HRNonCashBenefit">The hr non cash benefit.</param>
        /// <returns>HR_NONCASH_BENEFIT.</returns>
        public HR_NONCASH_BENEFIT ReadHRNonCashBenefitbyKey(string HRNonCashBenefit)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_NONCASH_BENEFIT.Where(c => c.NBEN_CODE == HRNonCashBenefit).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadHRNonCashBenefitbyKey : {0} ", ex.Message));
                return null;
            }
        }


        /// <summary>
        /// Return all HR Non Cash Benefit records
        /// </summary>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;HR_NONCASH_BENEFIT&gt;.</returns>
        public ResultDTO<HR_NONCASH_BENEFIT> ReadAllHRNonCashBenefit(int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    int totalcount = ctx.HR_NONCASH_BENEFIT.Count();
                    var result = ctx.HR_NONCASH_BENEFIT.OrderBy(i => i.NBEN_CODE).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<HR_NONCASH_BENEFIT>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on HR Non CASH_BENEFIT : {0} ", ex.Message));
                return new ResultDTO<HR_NONCASH_BENEFIT>();
            }
        }

        /// <summary>
        /// Execute the query and Return HRNonCashBenefit records
        /// </summary>
        /// <returns>IQueryable&lt;HR_NONCASH_BENEFIT&gt;.</returns>
        public IQueryable<HR_NONCASH_BENEFIT> ReadHRNonCashBenefit()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_NONCASH_BENEFIT.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadHRNonCashBenefit : {0} ", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Update HRNonCashBenefit record
        /// </summary>
        /// <param name="modifiedHRNonCashBenefit">The modified hr non cash benefit.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateHRNonCashBenefit(HR_NONCASH_BENEFIT modifiedHRNonCashBenefit)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        HR_NONCASH_BENEFIT original = ctx.HR_NONCASH_BENEFIT.Find(modifiedHRNonCashBenefit.NBEN_CODE);

                        if (original != null)
                        {
                            ctx.Entry(original).CurrentValues.SetValues(modifiedHRNonCashBenefit);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("AccountType with DocCode:{0} was not found.", modifiedHRNonCashBenefit.NBEN_CODE)
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
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new HRNonCashBenefit : {0} ", errorMsg));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = errorMsg };

            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateHRNonCashBenefit : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }
        /// <summary>
        /// Delete HRNonCashBenefit record
        /// </summary>
        /// <param name="deletingHRNonCashBenefit">The deleting hr non cash benefit.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteHRNonCashBenefit(HR_NONCASH_BENEFIT deletingHRNonCashBenefit)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        HR_NONCASH_BENEFIT original = ctx.HR_NONCASH_BENEFIT.Find(deletingHRNonCashBenefit.NBEN_CODE);

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
                                    string.Format("HRCashBenefit with HRNonCashBenefit:{0}  was not found.", deletingHRNonCashBenefit.NBEN_CODE)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteHRCashBenefit) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Hrs the non cash benefit exists.
        /// </summary>
        /// <param name="existsHRNonCashBenefit">The exists hr non cash benefit.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool HRNonCashBenefitExists(HR_NONCASH_BENEFIT existsHRNonCashBenefit)
        {
            try
            {

                using (var ctx = new MDMEntities())
                {
                    HR_NONCASH_BENEFIT original = ctx.HR_NONCASH_BENEFIT.Find(existsHRNonCashBenefit.NBEN_CODE);

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
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred HRNonCashBenefitExists : {0} ", ex.Message));
                return false;
            }
        }
        #endregion

        #region HR Employee Cash Benefits

        /// <summary>
        /// Queries the hr emp cash benefits.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;HR_EMP_CASH_BENEFIT&gt;.</returns>
        public ResultDTO<HR_EMP_CASH_BENEFIT> QueryHREmpCashBenefits(HREmpCashBenefitsQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.HR_EMP_CASH_BENEFIT.AsQueryable();

                    if (query.EmpCode != null)
                    {
                        c = c.Where(i => i.EMP_NUMBER == query.EmpCode);
                    }
                    if (query.BenefitCode != null)
                    {
                        c = c.Where(i => i.BEN_CODE == query.BenefitCode);
                    }


                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.EMP_NUMBER).Skip((pageNumber - 1) * pageSize).Take(pageSize).Include(s => s.HR_EMPLOYEE).Include(s => s.HR_CASH_BENEFIT).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<HR_EMP_CASH_BENEFIT>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on QueryHREmpCashBenefits : {0} ", ex.Message));
                return null;
            }
        }


        /// <summary>
        /// Creates the hr emp cash benefits.
        /// </summary>
        /// <param name="new_FIN_HREmpCashBenefits">The new_ fi n_ hr emp cash benefits.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateHREmpCashBenefits(HR_EMP_CASH_BENEFIT new_FIN_HREmpCashBenefits)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {

                        var empCashBenefits = ctx.HR_CASH_BENEFIT.Find(new_FIN_HREmpCashBenefits.HR_CASH_BENEFIT.BEN_CODE);
                        var emp = ctx.HR_EMPLOYEE.Find(new_FIN_HREmpCashBenefits.HR_EMPLOYEE.EMP_NUMBER);
                        new_FIN_HREmpCashBenefits.HR_EMPLOYEE = emp;
                        new_FIN_HREmpCashBenefits.HR_CASH_BENEFIT = empCashBenefits;

                        ctx.HR_EMP_CASH_BENEFIT.Add(new_FIN_HREmpCashBenefits);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new HREmpCashBenefits : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Reads the hr emp cash benefitsby key.
        /// </summary>
        /// <param name="EmpCode">The emp code.</param>
        /// <param name="BenCode">The ben code.</param>
        /// <returns>HR_EMP_CASH_BENEFIT.</returns>
        public HR_EMP_CASH_BENEFIT ReadHREmpCashBenefitsbyKey(string EmpCode, string BenCode)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMP_CASH_BENEFIT.Where(c => c.EMP_NUMBER == EmpCode && c.BEN_CODE == BenCode).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadHREmpCashBenefitsbyKey : {0} ", ex.Message));
                return null;
            }
        }


        /// <summary>
        /// Return all HR Employee Cash Benefit records
        /// </summary>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;HR_EMP_CASH_BENEFIT&gt;.</returns>
        public ResultDTO<HR_EMP_CASH_BENEFIT> ReadAllHREmpCashBenefit(int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    int totalcount = ctx.HR_EMP_CASH_BENEFIT.Count();
                    var result = ctx.HR_EMP_CASH_BENEFIT.OrderBy(i => i.BEN_CODE).Skip((pageNumber - 1) * pageSize).Take(pageSize).Include(s => s.HR_EMPLOYEE).Include(s => s.HR_CASH_BENEFIT).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<HR_EMP_CASH_BENEFIT>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on HR_CASH_BENEFIT : {0} ", ex.Message));
                return new ResultDTO<HR_EMP_CASH_BENEFIT>();
            }
        }

        /// <summary>
        /// Execute the query and Return HR Employee Cash Benefit records
        /// </summary>
        /// <returns>IQueryable&lt;HR_EMP_CASH_BENEFIT&gt;.</returns>
        public IQueryable<HR_EMP_CASH_BENEFIT> ReadHREmpCashBenefits()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMP_CASH_BENEFIT.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadHREmpCashBenefits : {0} ", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Update HR EMP CASH BENEFIT record
        /// </summary>
        /// <param name="modifiedHREmpCashBenefits">The modified hr emp cash benefits.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateHREmpCashBenefits(HR_EMP_CASH_BENEFIT modifiedHREmpCashBenefits)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        HR_EMP_CASH_BENEFIT original = ctx.HR_EMP_CASH_BENEFIT.Find(modifiedHREmpCashBenefits.EMP_NUMBER, modifiedHREmpCashBenefits.BEN_CODE);

                        if (original != null)
                        {
                            ctx.Entry(original).CurrentValues.SetValues(modifiedHREmpCashBenefits);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("HR Emp Cash Benefits with Employee Code:{0} Benefit Code:{1} was not found.", modifiedHREmpCashBenefits.EMP_NUMBER, modifiedHREmpCashBenefits.BEN_CODE)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateHREmpCashBenefits : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Delete HR Emp Cash Benefit record
        /// </summary>
        /// <param name="deletingHREmpCashBenefits">The deleting hr emp cash benefits.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteHREmpCashBenefits(HR_EMP_CASH_BENEFIT deletingHREmpCashBenefits)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        HR_EMP_CASH_BENEFIT original = ctx.HR_EMP_CASH_BENEFIT.Find(deletingHREmpCashBenefits.EMP_NUMBER, deletingHREmpCashBenefits.BEN_CODE);

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
                                    string.Format("HR Emp Cash Benefits with Employee Code:{0} Benefit Code:{1} was not found.", deletingHREmpCashBenefits.EMP_NUMBER, deletingHREmpCashBenefits.BEN_CODE)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteHREmpCashBenefits) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Hrs the emp cash benefits exists.
        /// </summary>
        /// <param name="existsHREmpCashBenefits">The exists hr emp cash benefits.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool HREmpCashBenefitsExists(HR_EMP_CASH_BENEFIT existsHREmpCashBenefits)
        {
            try
            {

                using (var ctx = new MDMEntities())
                {
                    HR_EMP_CASH_BENEFIT original = ctx.HR_EMP_CASH_BENEFIT.Find(existsHREmpCashBenefits.EMP_NUMBER, existsHREmpCashBenefits.BEN_CODE);

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

        #region HR Employee Non-Cash Benefits

        /// <summary>
        /// Queries the hr emp non cash benefits.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;HR_EMP_NONCASH_BENEFIT&gt;.</returns>
        public ResultDTO<HR_EMP_NONCASH_BENEFIT> QueryHREmpNonCashBenefits(HREmpNonCashBenefitsQuery query, int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    var c = ctx.HR_EMP_NONCASH_BENEFIT.AsQueryable();

                    if (query.EmpCode != null)
                    {
                        c = c.Where(i => i.EMP_NUMBER == query.EmpCode);
                    }
                    if (query.NonBenefitCode != null)
                    {
                        c = c.Where(i => i.NBEN_CODE == query.NonBenefitCode);
                    }


                    int totalcount = c.Count();
                    var result = c.OrderBy(i => i.EMP_NUMBER).Skip((pageNumber - 1) * pageSize).Take(pageSize).Include(s => s.HR_EMPLOYEE).Include(s => s.HR_NONCASH_BENEFIT).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<HR_EMP_NONCASH_BENEFIT>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on QueryHREmpNonCashBenefits : {0} ", ex.Message));
                return null;
            }
        }


        /// <summary>
        /// Creates the hr emp non cash benefits.
        /// </summary>
        /// <param name="new_FIN_HREmpNonCashBenefits">The new_ fi n_ hr emp non cash benefits.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck CreateHREmpNonCashBenefits(HR_EMP_NONCASH_BENEFIT new_FIN_HREmpNonCashBenefits)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {

                        var empNonCashBenefits = ctx.HR_NONCASH_BENEFIT.Find(new_FIN_HREmpNonCashBenefits.HR_NONCASH_BENEFIT.NBEN_CODE);
                        var emp = ctx.HR_EMPLOYEE.Find(new_FIN_HREmpNonCashBenefits.HR_EMPLOYEE.EMP_NUMBER);
                        new_FIN_HREmpNonCashBenefits.HR_EMPLOYEE = emp;
                        new_FIN_HREmpNonCashBenefits.HR_NONCASH_BENEFIT = empNonCashBenefits;

                        ctx.HR_EMP_NONCASH_BENEFIT.Add(new_FIN_HREmpNonCashBenefits);
                        ctx.SaveChanges();
                    }
                    transaction.Complete();
                }
                return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on creating new HREmpNonCashBenefits : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Reads the hr emp non cash benefitsby key.
        /// </summary>
        /// <param name="EmpCode">The emp code.</param>
        /// <param name="NBenCode">The n ben code.</param>
        /// <returns>HR_EMP_NONCASH_BENEFIT.</returns>
        public HR_EMP_NONCASH_BENEFIT ReadHREmpNonCashBenefitsbyKey(string EmpCode, string NBenCode)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMP_NONCASH_BENEFIT.Where(c => c.EMP_NUMBER == EmpCode && c.NBEN_CODE == NBenCode).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadHREmpNonCashBenefitsbyKey : {0} ", ex.Message));
                return null;
            }
        }


        /// <summary>
        /// Return all HR Emp NonCash Benefits records
        /// </summary>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>ResultDTO&lt;HR_EMP_NONCASH_BENEFIT&gt;.</returns>
        public ResultDTO<HR_EMP_NONCASH_BENEFIT> ReadAllHREmpNonCashBenefit(int pageSize, int pageNumber)
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    int totalcount = ctx.HR_EMP_NONCASH_BENEFIT.Count();
                    var result = ctx.HR_EMP_NONCASH_BENEFIT.OrderBy(i => i.NBEN_CODE).Skip((pageNumber - 1) * pageSize).Take(pageSize).Include(s => s.HR_EMPLOYEE).Include(s => s.HR_NONCASH_BENEFIT).ToArray();
                    int localcount = result.Count();
                    var dto = new ResultDTO<HR_EMP_NONCASH_BENEFIT>() { Result = result, TotalResultCount = totalcount, LocalResultCount = localcount };
                    return dto;

                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on HR_EMP_NONCASH_BENEFIT : {0} ", ex.Message));
                return new ResultDTO<HR_EMP_NONCASH_BENEFIT>();
            }
        }

        /// <summary>
        /// Execute the query and Return HR Emp NonCash Benefits records
        /// </summary>
        /// <returns>IQueryable&lt;HR_EMP_NONCASH_BENEFIT&gt;.</returns>
        public IQueryable<HR_EMP_NONCASH_BENEFIT> ReadHREmpNonCashBenefits()
        {
            try
            {
                using (var ctx = new MDMEntities())
                {
                    return ctx.HR_EMP_NONCASH_BENEFIT.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on ReadHREmpNonCashBenefits : {0} ", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Update HR EMP CASH BENEFIT record
        /// </summary>
        /// <param name="modifiedHREmpNonCashBenefits">The modified hr emp non cash benefits.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck UpdateHREmpNonCashBenefits(HR_EMP_NONCASH_BENEFIT modifiedHREmpNonCashBenefits)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        HR_EMP_NONCASH_BENEFIT original = ctx.HR_EMP_NONCASH_BENEFIT.Find(modifiedHREmpNonCashBenefits.EMP_NUMBER, modifiedHREmpNonCashBenefits.NBEN_CODE);

                        if (original != null)
                        {
                            ctx.Entry(original).CurrentValues.SetValues(modifiedHREmpNonCashBenefits);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return new ApiAck
                            {
                                CallStatus = EApiCallStatus.ValidationFailed,
                                ReturnedMessage =
                                    string.Format("HR Emp Non Cash Benefits with Employee Code:{0} Benefit Code:{1} was not found.", modifiedHREmpNonCashBenefits.EMP_NUMBER, modifiedHREmpNonCashBenefits.NBEN_CODE)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on UpdateHREmpNonCashBenefits : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Delete HR Emp Non Cash Benefit record
        /// </summary>
        /// <param name="deletingHREmpNonCashBenefits">The deleting hr emp non cash benefits.</param>
        /// <returns>ApiAck.</returns>
        public ApiAck DeleteHREmpNonCashBenefits(HR_EMP_NONCASH_BENEFIT deletingHREmpNonCashBenefits)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    using (var ctx = new MDMEntities())
                    {
                        HR_EMP_NONCASH_BENEFIT original = ctx.HR_EMP_NONCASH_BENEFIT.Find(deletingHREmpNonCashBenefits.EMP_NUMBER, deletingHREmpNonCashBenefits.NBEN_CODE);

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
                                    string.Format("HR Emp Non Cash Benefits with Employee Code:{0} Benefit Code:{1} was not found.", deletingHREmpNonCashBenefits.EMP_NUMBER, deletingHREmpNonCashBenefits.NBEN_CODE)
                            };
                        }
                    }
                    transaction.Complete();
                    return new ApiAck { CallStatus = EApiCallStatus.Success, ReturnedMessage = string.Empty };
                }
            }
            catch (Exception ex)
            {
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred on DeleteHREmpNonCashBenefits) : {0} ", ex.Message));
                return new ApiAck { CallStatus = EApiCallStatus.Error, ReturnedMessage = ex.GetBaseException().Message };
            }
        }

        /// <summary>
        /// Hrs the emp non cash benefits exists.
        /// </summary>
        /// <param name="existsHREmpNonCashBenefits">The exists hr emp non cash benefits.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool HREmpNonCashBenefitsExists(HR_EMP_NONCASH_BENEFIT existsHREmpNonCashBenefits)
        {
            try
            {

                using (var ctx = new MDMEntities())
                {
                    HR_EMP_NONCASH_BENEFIT original = ctx.HR_EMP_NONCASH_BENEFIT.Find(existsHREmpNonCashBenefits.EMP_NUMBER, existsHREmpNonCashBenefits.NBEN_CODE);

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
                APIErrorLog.LogAPIError("FM100", string.Format("Unexpected error has occurred HREmpNonCashBenefits : {0} ", ex.Message));
                return false;
            }
        }
        #endregion
    
    }
}
