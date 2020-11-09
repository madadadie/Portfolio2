﻿using System;
using DataLayer.FromSQL;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataLayer
{
    public class DatabaseContext : DbContext
    {
        private readonly string _connectionString;
        public DatabaseContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

        public DbSet<User> Users { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<TitleGenre> TitleGenres { get; set; }
        public DbSet<TitleBasics> Titles { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<UserRates> UserRates { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<TitlePrincipal> TitlePrincipals { get; set; }
        public DbSet<KnownFor> KnownFor { get; set; }
        public DbSet<TitleAka> TitleAkas { get; set; }
        public DbSet<SearchHistory> SearchHistory { get; set; }
        public DbSet<OmdbData> OmdbData { get; set; }
        public DbSet<WordIndex> WordIndex { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<Actors> Actors { get; set; }
        public DbSet<CoPlayers> CoPlayers { get; set; }
        public DbSet<ProductionTeam> ProductionTeam { get; set; }
        public DbSet<TitleBestMatch> TitleBestMatch { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(loggerFactory);
            optionsBuilder.UseNpgsql(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //table users
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<User>().HasKey(u => u.UserId);
            modelBuilder.Entity<User>().Property(u => u.UserId).HasColumnName("iduser");
            modelBuilder.Entity<User>().Property(u => u.FirstName).HasColumnName("f_name");
            modelBuilder.Entity<User>().Property(u => u.LastName).HasColumnName("l_name");
            modelBuilder.Entity<User>().Property(u => u.BirthDay).HasColumnName("birthday");
            modelBuilder.Entity<User>().Property(u => u.IsStaff).HasColumnName("isstaff");
            modelBuilder.Entity<User>().Property(u => u.Email).HasColumnName("email");
            modelBuilder.Entity<User>().Property(u => u.Password).HasColumnName("password");
            modelBuilder.Entity<User>().Property(u => u.UserName).HasColumnName("u_name");
            modelBuilder.Entity<User>().Property(u => u.AddressId).HasColumnName("idaddress");



            //table address
            modelBuilder.Entity<Address>().ToTable("address");
            modelBuilder.Entity<Address>().HasKey(a => a.Id);
            modelBuilder.Entity<Address>().Property(a => a.Id).HasColumnName("idaddress");
            modelBuilder.Entity<Address>().Property(a => a.StreetNumber).HasColumnName("streetnumber");
            modelBuilder.Entity<Address>().Property(a => a.StreetName).HasColumnName("streetname");
            modelBuilder.Entity<Address>().Property(a => a.ZipCode).HasColumnName("zipcode");
            modelBuilder.Entity<Address>().Property(a => a.City).HasColumnName("city");
            modelBuilder.Entity<Address>().Property(a => a.Country).HasColumnName("country");


            //table titlebasics
            modelBuilder.Entity<TitleBasics>().ToTable("titlebasics");
            modelBuilder.Entity<TitleBasics>().HasKey(t => t.Const);
            modelBuilder.Entity<TitleBasics>().Property(t => t.Const).HasColumnName("titleconst");
            modelBuilder.Entity<TitleBasics>().Property(t => t.Type).HasColumnName("titletype");
            modelBuilder.Entity<TitleBasics>().Property(t => t.PrimaryTitle).HasColumnName("primarytitle");
            modelBuilder.Entity<TitleBasics>().Property(t => t.IsAdult).HasColumnName("isadult");
            modelBuilder.Entity<TitleBasics>().Property(t => t.StartYear).HasColumnName("startyear");
            modelBuilder.Entity<TitleBasics>().Property(t => t.EndYear).HasColumnName("endyear");
            modelBuilder.Entity<TitleBasics>().Property(t => t.Runtime).HasColumnName("runtimeinmins");
            modelBuilder.Entity<TitleBasics>()
                .HasOne(t => t.Rating)
                .WithOne(tr => tr.Title)
                .HasForeignKey<TitleRating>(t => t.Const);
            modelBuilder.Entity<TitleBasics>()
                .HasOne(t => t.OmdbData)
                .WithOne(od => od.Title)
                .HasForeignKey<OmdbData>(t => t.TitleConst);

            //Genres
            modelBuilder.Entity<Genre>().ToTable("genre");
            modelBuilder.Entity<Genre>().HasKey(g => g.Id);
            modelBuilder.Entity<Genre>().Property(g => g.Id).HasColumnName("idgenre");
            modelBuilder.Entity<Genre>().Property(g => g.Name).HasColumnName("name");


            //titleGenre
            modelBuilder.Entity<TitleGenre>().ToTable("titlegenre");
            modelBuilder.Entity<TitleGenre>().HasKey(tg => new { tg.TitleConst, tg.IdGenre });
            modelBuilder.Entity<TitleGenre>().Property(tg => tg.TitleConst).HasColumnName("titleconst");
            modelBuilder.Entity<TitleGenre>().Property(tg => tg.IdGenre).HasColumnName("idgenre");
            modelBuilder.Entity<TitleGenre>()
                .HasOne(tg => tg.Title)
                .WithMany(t => t.TitleGenres)
                .HasForeignKey(tg => tg.TitleConst);
            modelBuilder.Entity<TitleGenre>()
               .HasOne(tg => tg.Genre)
               .WithMany(g => g.TitleGenres)
               .HasForeignKey(tg => tg.IdGenre);

            //titleRating
            modelBuilder.Entity<TitleRating>().ToTable("titleratings");
            modelBuilder.Entity<TitleRating>().HasKey(tr => tr.Const);
            modelBuilder.Entity<TitleRating>().Property(tr => tr.Const).HasColumnName("titleconst");
            modelBuilder.Entity<TitleRating>().Property(tr => tr.Average).HasColumnName("avgrating");
            modelBuilder.Entity<TitleRating>().Property(tr => tr.NumVotes).HasColumnName("numvotes");

            //userRates  
            modelBuilder.Entity<UserRates>().ToTable("rates");
            modelBuilder.Entity<UserRates>().HasKey(ur => new { ur.UserId, ur.TitleConst });
            modelBuilder.Entity<UserRates>().Property(ur => ur.UserId).HasColumnName("iduser");
            modelBuilder.Entity<UserRates>().Property(ur => ur.TitleConst).HasColumnName("titleconst");
            modelBuilder.Entity<UserRates>().Property(ur => ur.NumericR).HasColumnName("numericrating");
            modelBuilder.Entity<UserRates>().Property(ur => ur.VerbalR).HasColumnName("verbalrating");
            modelBuilder.Entity<UserRates>().Property(ur => ur.Date).HasColumnName("r_date");
            modelBuilder.Entity<UserRates>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRates)
                .HasForeignKey(ur => ur.UserId);
            modelBuilder.Entity<UserRates>()
               .HasOne(ur => ur.Title)
               .WithMany(tr => tr.UserRates)
               .HasForeignKey(ur => ur.TitleConst);

            //persons
            modelBuilder.Entity<Person>().ToTable("persons");
            modelBuilder.Entity<Person>().HasKey(p => p.NameConst);
            modelBuilder.Entity<Person>().Property(p => p.NameConst).HasColumnName("nameconst");
            modelBuilder.Entity<Person>().Property(p => p.Name).HasColumnName("primaryname");
            modelBuilder.Entity<Person>().Property(p => p.BirthYear).HasColumnName("birthyear");
            modelBuilder.Entity<Person>().Property(p => p.DeathYear).HasColumnName("deathyear");

            //titleprincipals
            modelBuilder.Entity<TitlePrincipal>().ToTable("titleprincipals");
            modelBuilder.Entity<TitlePrincipal>().HasKey(tp => new { tp.TitleConst, tp.NameConst, tp.Ordering });
            modelBuilder.Entity<TitlePrincipal>().Property(tp => tp.TitleConst).HasColumnName("titleconst");
            modelBuilder.Entity<TitlePrincipal>().Property(tp => tp.NameConst).HasColumnName("nameconst");
            modelBuilder.Entity<TitlePrincipal>().Property(tp => tp.Ordering).HasColumnName("ordering");
            modelBuilder.Entity<TitlePrincipal>().Property(tp => tp.Category).HasColumnName("category");
            modelBuilder.Entity<TitlePrincipal>().Property(tp => tp.Job).HasColumnName("job");
            modelBuilder.Entity<TitlePrincipal>().Property(tp => tp.Characters).HasColumnName("characters");
            modelBuilder.Entity<TitlePrincipal>()
                .HasOne(tp => tp.Title)
                .WithMany(t => t.TitlePrincipals)
                .HasForeignKey(tp => tp.TitleConst);
            modelBuilder.Entity<TitlePrincipal>()
                .HasOne(tp => tp.Person)
                .WithMany(p => p.TitlePrincipals)
                .HasForeignKey(tp => tp.NameConst);

            //knownfor
            modelBuilder.Entity<KnownFor>().ToTable("knownfor");
            modelBuilder.Entity<KnownFor>().HasKey(kf => new { kf.TitleConst, kf.NameConst });
            modelBuilder.Entity<KnownFor>().Property(kf => kf.TitleConst).HasColumnName("titleconst");
            modelBuilder.Entity<KnownFor>().Property(kf => kf.NameConst).HasColumnName("nameconst");
            modelBuilder.Entity<KnownFor>()
                .HasOne(kn => kn.Title)
                .WithMany(t => t.KnownFor)
                .HasForeignKey(kn => kn.TitleConst);
            modelBuilder.Entity<KnownFor>()
                .HasOne(kn => kn.Person)
                .WithMany(p => p.KnownFor)
                .HasForeignKey(kn => kn.NameConst);

            //TitleAkas
            modelBuilder.Entity<TitleAka>().ToTable("titleakas");
            modelBuilder.Entity<TitleAka>().HasKey(ta => new { ta.Titleconst, ta.Ordering });
            modelBuilder.Entity<TitleAka>().Property(ta => ta.Titleconst).HasColumnName("titleconst");
            modelBuilder.Entity<TitleAka>().Property(ta => ta.Ordering).HasColumnName("ordering");
            modelBuilder.Entity<TitleAka>().Property(ta => ta.Alias).HasColumnName("title");
            modelBuilder.Entity<TitleAka>().Property(ta => ta.IsOriginal).HasColumnName("isoriginal");
            modelBuilder.Entity<TitleAka>().Property(ta => ta.Region).HasColumnName("region");
            modelBuilder.Entity<TitleAka>().Property(ta => ta.Types).HasColumnName("types");
            modelBuilder.Entity<TitleAka>().Property(ta => ta.Attributes).HasColumnName("attributes");

            //SearchHistory
            modelBuilder.Entity<SearchHistory>().ToTable("searchhistory");
            modelBuilder.Entity<SearchHistory>().HasKey(sh => sh.SearchId);
            modelBuilder.Entity<SearchHistory>().Property(sh => sh.SearchId).HasColumnName("idsearch");
            modelBuilder.Entity<SearchHistory>().Property(sh => sh.UserId).HasColumnName("iduser");
            modelBuilder.Entity<SearchHistory>().Property(sh => sh.Word).HasColumnName("word");
            modelBuilder.Entity<SearchHistory>().Property(sh => sh.Date).HasColumnName("h_date");

            //Episode
            modelBuilder.Entity<Episode>().ToTable("episode");
            modelBuilder.Entity<Episode>().HasKey(e => new { e.TitleConst, e.SerieId });
            modelBuilder.Entity<Episode>().Property(e => e.TitleConst).HasColumnName("titleconst");
            modelBuilder.Entity<Episode>().Property(e => e.SerieId).HasColumnName("parenttconst");
            modelBuilder.Entity<Episode>().Property(e => e.Season).HasColumnName("seasonnumber");
            modelBuilder.Entity<Episode>().Property(e => e.NumEpisode).HasColumnName("episodenumber");
            modelBuilder.Entity<Episode>()
                .HasOne(e => e.Title)
                .WithMany(t => t.Episodes)
                .HasForeignKey(e => e.TitleConst);
            modelBuilder.Entity<Episode>()
                .HasOne(e => e.Title)
                .WithMany(t => t.Episodes)
                .HasForeignKey(e => e.SerieId);

            //OmdbData
            modelBuilder.Entity<OmdbData>().ToTable("omdbdata");
            modelBuilder.Entity<OmdbData>().HasKey(od => od.TitleConst);
            modelBuilder.Entity<OmdbData>().Property(od => od.TitleConst).HasColumnName("titleconst");
            modelBuilder.Entity<OmdbData>().Property(od => od.Poster).HasColumnName("poster");
            modelBuilder.Entity<OmdbData>().Property(od => od.Awards).HasColumnName("awards");
            modelBuilder.Entity<OmdbData>().Property(od => od.Plot).HasColumnName("plot");

            //WordIndex
            modelBuilder.Entity<WordIndex>().ToTable("wordindex");
            modelBuilder.Entity<WordIndex>().HasKey(wi => new { wi.TitleConst, wi.Word, wi.Field });
            modelBuilder.Entity<WordIndex>().Property(wi => wi.TitleConst).HasColumnName("titleconst");
            modelBuilder.Entity<WordIndex>().Property(wi => wi.Word).HasColumnName("word");
            modelBuilder.Entity<WordIndex>().Property(wi => wi.Field).HasColumnName("field");
            modelBuilder.Entity<WordIndex>().Property(wi => wi.Lexeme).HasColumnName("lexeme");


            /*
             * Database functions results
             */
            //User Role
            modelBuilder.Entity<UserRole>().HasNoKey();
            modelBuilder.Entity<UserRole>().Property(ur => ur.UserId).HasColumnName("iduser");
            modelBuilder.Entity<UserRole>().Property(ur => ur.IsStaff).HasColumnName("isstaff");

            //Actor
            modelBuilder.Entity<Actors>().HasNoKey();
            modelBuilder.Entity<Actors>().Property(a => a.NameConst).HasColumnName("nameconst");
            modelBuilder.Entity<Actors>().Property(a => a.Name).HasColumnName("primaryname");
            modelBuilder.Entity<Actors>().Property(a => a.Gender).HasColumnName("gender");

            //CoPlayers
            modelBuilder.Entity<CoPlayers>().HasNoKey();
            modelBuilder.Entity<CoPlayers>().Property(cp => cp.NameConst).HasColumnName("nameconst");
            modelBuilder.Entity<CoPlayers>().Property(cp => cp.Name).HasColumnName("pname");
            modelBuilder.Entity<CoPlayers>().Property(cp => cp.Frequency).HasColumnName("frequency");

            //Production team
            modelBuilder.Entity<ProductionTeam>().HasNoKey();
            modelBuilder.Entity<ProductionTeam>().Property(pt => pt.NameConst).HasColumnName("nameconst");
            modelBuilder.Entity<ProductionTeam>().Property(pt => pt.Name).HasColumnName("primaryname");
            modelBuilder.Entity<ProductionTeam>().Property(pt => pt.Role).HasColumnName("role");

            //Title best match
            modelBuilder.Entity<TitleBestMatch>().HasNoKey();
            modelBuilder.Entity<TitleBestMatch>().Property(tbm => tbm.TitleConst).HasColumnName("titleconst");
            modelBuilder.Entity<TitleBestMatch>().Property(tbm => tbm.Title).HasColumnName("primarytitle");
            modelBuilder.Entity<TitleBestMatch>().Property(tbm => tbm.Rank).HasColumnName("rank");

        }
    }
}
