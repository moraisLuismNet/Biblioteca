﻿// <auto-generated />
using Biblioteca.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Biblioteca.Migrations
{
    [DbContext(typeof(BibliotecaContext))]
    [Migration("20250218064624_AutoresEditorialesLibros")]
    partial class AutoresEditorialesLibros
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Biblioteca.Models.Autor", b =>
                {
                    b.Property<int>("IdAutor")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdAutor"));

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdAutor");

                    b.ToTable("Autores");
                });

            modelBuilder.Entity("Biblioteca.Models.Editorial", b =>
                {
                    b.Property<int>("IdEditorial")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdEditorial"));

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdEditorial");

                    b.ToTable("Editoriales");
                });

            modelBuilder.Entity("Biblioteca.Models.Libro", b =>
                {
                    b.Property<int>("IdLibro")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdLibro"));

                    b.Property<int>("AutorId")
                        .HasColumnType("int");

                    b.Property<bool>("Descatalogado")
                        .HasColumnType("bit");

                    b.Property<int>("EditorialId")
                        .HasColumnType("int");

                    b.Property<string>("FotoPortada")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Paginas")
                        .HasColumnType("int");

                    b.Property<decimal>("Precio")
                        .HasColumnType("decimal(9,2)");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdLibro");

                    b.HasIndex("AutorId");

                    b.HasIndex("EditorialId");

                    b.ToTable("Libros");
                });

            modelBuilder.Entity("Biblioteca.Models.Libro", b =>
                {
                    b.HasOne("Biblioteca.Models.Autor", "Autor")
                        .WithMany("Libros")
                        .HasForeignKey("AutorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Biblioteca.Models.Editorial", "Editorial")
                        .WithMany("Libros")
                        .HasForeignKey("EditorialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Autor");

                    b.Navigation("Editorial");
                });

            modelBuilder.Entity("Biblioteca.Models.Autor", b =>
                {
                    b.Navigation("Libros");
                });

            modelBuilder.Entity("Biblioteca.Models.Editorial", b =>
                {
                    b.Navigation("Libros");
                });
#pragma warning restore 612, 618
        }
    }
}
