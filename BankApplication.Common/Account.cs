using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using BankApplication.Common;
namespace BankApplication.Common
{
    public abstract class Account:IAccount
    {
        protected string accNo;
        protected string name;
        protected string pin;
        protected bool active;
        protected DateTime dateOfOpening;
        protected double balance;
        protected PrivilegeType privilegeType;
        public IPolicy Policy { get; set; }
        

        // Getters and Setters for instance variables
        public string AccNo { get => accNo; set => accNo = value; }
        public string Name { get => name; set => name = value; }
        public string Pin { get => pin; set => pin = value; }
        public bool Active { get => active; set => active = value; }
        public DateTime DateOfOpening { get => dateOfOpening; set => dateOfOpening = value; }
        public double Balance { get => balance; set => balance = value; }
        public PrivilegeType PrivilegeType { get => privilegeType; set => privilegeType = value; }

        public abstract string GetAccType();
        public virtual bool Open()
        {
            AccNo = IDGenerator.GenerateAccNo(GetAccType());
            Active = true;
            DateOfOpening = DateTime.Now;
            return true;
        }

        public virtual bool Close()
        {
            Active = false;
            Balance = 0;
            return true;
        }
        
    }

}
