﻿// <auto-generated />
using System;
using AccountDB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AccountDB.Migrations
{
    [DbContext(typeof(AccountContext))]
    [Migration("20231106185609_InitDB")]
    partial class InitDB
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AccountDB.Cuenta", b =>
                {
                    b.Property<int>("NumeroCuenta")
                        .HasColumnType("int");

                    b.Property<int?>("ClientID")
                        .HasColumnType("int");

                    b.Property<bool>("Estado")
                        .HasColumnType("bit");

                    b.Property<int>("SaldoInicial")
                        .HasColumnType("int");

                    b.Property<string>("TipoCuenta")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("NumeroCuenta");

                    b.ToTable("Cuentas");
                });

            modelBuilder.Entity("AccountDB.Movimiento", b =>
                {
                    b.Property<int>("MovimientoID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MovimientoID"));

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<int>("NumeroCuenta")
                        .HasColumnType("int");

                    b.Property<string>("Saldo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Tipo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Valor")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MovimientoID");

                    b.HasIndex("NumeroCuenta");

                    b.ToTable("Movimientos");
                });

            modelBuilder.Entity("AccountDB.Movimiento", b =>
                {
                    b.HasOne("AccountDB.Cuenta", "Cuenta")
                        .WithMany()
                        .HasForeignKey("NumeroCuenta")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cuenta");
                });
#pragma warning restore 612, 618
        }
    }
}
