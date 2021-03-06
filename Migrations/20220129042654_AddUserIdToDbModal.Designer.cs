// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ModalBuilderUtil;

#nullable disable

namespace ModalBuilderUtil.Migrations
{
    [DbContext(typeof(OddlyFluffyDbContext))]
    [Migration("20220129042654_AddUserIdToDbModal")]
    partial class AddUserIdToDbModal
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.1");

            modelBuilder.Entity("ModalBuilderUtil.DbActionRow", b =>
                {
                    b.Property<int>("DbActionRowId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DBModalId")
                        .HasColumnType("INTEGER");

                    b.HasKey("DbActionRowId");

                    b.HasIndex("DBModalId");

                    b.ToTable("ActionRows");
                });

            modelBuilder.Entity("ModalBuilderUtil.DbComponent", b =>
                {
                    b.Property<int?>("DbComponentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ButtonStyle")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CustomId")
                        .HasColumnType("TEXT");

                    b.Property<int>("DbActionRowId")
                        .HasColumnType("INTEGER");

                    b.Property<bool?>("Disabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Emote")
                        .HasColumnType("TEXT");

                    b.Property<string>("Label")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Max")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Min")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Placeholder")
                        .HasColumnType("TEXT");

                    b.Property<bool?>("Required")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TextInputStyle")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Url")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("DbComponentId");

                    b.HasIndex("DbActionRowId");

                    b.ToTable("Components");
                });

            modelBuilder.Entity("ModalBuilderUtil.DbModal", b =>
                {
                    b.Property<int>("DbModalId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CustomId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.Property<ulong>("UserID")
                        .HasColumnType("INTEGER");

                    b.HasKey("DbModalId");

                    b.ToTable("Modals");
                });

            modelBuilder.Entity("ModalBuilderUtil.DbSelectOption", b =>
                {
                    b.Property<int?>("DbSelectOptionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("DbComponentId")
                        .HasColumnType("INTEGER");

                    b.Property<bool?>("Default")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Emote")
                        .HasColumnType("TEXT");

                    b.Property<string>("Label")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("DbSelectOptionId");

                    b.HasIndex("DbComponentId");

                    b.ToTable("SelectOptions");
                });

            modelBuilder.Entity("ModalBuilderUtil.DbActionRow", b =>
                {
                    b.HasOne("ModalBuilderUtil.DbModal", "Modal")
                        .WithMany("ActionRows")
                        .HasForeignKey("DBModalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Modal");
                });

            modelBuilder.Entity("ModalBuilderUtil.DbComponent", b =>
                {
                    b.HasOne("ModalBuilderUtil.DbActionRow", "DbActionRow")
                        .WithMany("Components")
                        .HasForeignKey("DbActionRowId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DbActionRow");
                });

            modelBuilder.Entity("ModalBuilderUtil.DbSelectOption", b =>
                {
                    b.HasOne("ModalBuilderUtil.DbComponent", null)
                        .WithMany("SelectOptions")
                        .HasForeignKey("DbComponentId");
                });

            modelBuilder.Entity("ModalBuilderUtil.DbActionRow", b =>
                {
                    b.Navigation("Components");
                });

            modelBuilder.Entity("ModalBuilderUtil.DbComponent", b =>
                {
                    b.Navigation("SelectOptions");
                });

            modelBuilder.Entity("ModalBuilderUtil.DbModal", b =>
                {
                    b.Navigation("ActionRows");
                });
#pragma warning restore 612, 618
        }
    }
}
