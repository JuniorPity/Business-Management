﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BusinessManagement.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BusinessDataEntities : DbContext
    {
        public BusinessDataEntities()
            : base("name=BusinessDataEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<TimeEvent> TimeEvents { get; set; }
        public virtual DbSet<Badge> Badges { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<InviteCode> InviteCodes { get; set; }
        public virtual DbSet<UserImage> UserImages { get; set; }
    }
}
