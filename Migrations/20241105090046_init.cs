﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnet_cyberpunk_challenge_5.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArasakaClusters",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    clusterName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    nodeCount = table.Column<int>(type: "int", nullable: false),
                    cpuCores = table.Column<int>(type: "int", nullable: false),
                    memoryGb = table.Column<int>(type: "int", nullable: false),
                    storageTb = table.Column<int>(type: "int", nullable: false),
                    creationDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    environment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    kubernetesVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    region = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArasakaClusters", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    publicKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    architecture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    processorType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    region = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    athenaAccessKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    clusterId = table.Column<int>(type: "int", nullable: false),
                    ArasakaClusterid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.id);
                    table.ForeignKey(
                        name: "FK_Devices_ArasakaClusters_ArasakaClusterid",
                        column: x => x.ArasakaClusterid,
                        principalTable: "ArasakaClusters",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "AthenaDataEvents",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<int>(type: "int", nullable: false),
                    ipAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    macAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    eventTimestamp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    eventType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    severity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    userAgent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    deviceBrand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    deviceModel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    osVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    appName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    appVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    errorCode = table.Column<int>(type: "int", nullable: false),
                    errorMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    responseTime = table.Column<float>(type: "real", nullable: false),
                    success = table.Column<bool>(type: "bit", nullable: false),
                    deviceId = table.Column<int>(type: "int", nullable: false),
                    ArasakaDeviceid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AthenaDataEvents", x => x.id);
                    table.ForeignKey(
                        name: "FK_AthenaDataEvents_Devices_ArasakaDeviceid",
                        column: x => x.ArasakaDeviceid,
                        principalTable: "Devices",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "MemoryMappings",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    memoryType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    memorySizeGb = table.Column<float>(type: "real", nullable: false),
                    memorySpeedMhz = table.Column<int>(type: "int", nullable: false),
                    memoryTechnology = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    memoryLatency = table.Column<int>(type: "int", nullable: false),
                    memoryVoltage = table.Column<float>(type: "real", nullable: false),
                    memoryFormFactor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    memoryEccSupport = table.Column<bool>(type: "bit", nullable: false),
                    memoryHeatSpreader = table.Column<bool>(type: "bit", nullable: false),
                    memoryWarrantyYears = table.Column<int>(type: "int", nullable: false),
                    deviceId = table.Column<int>(type: "int", nullable: false),
                    ArasakaDeviceid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemoryMappings", x => x.id);
                    table.ForeignKey(
                        name: "FK_MemoryMappings_Devices_ArasakaDeviceid",
                        column: x => x.ArasakaDeviceid,
                        principalTable: "Devices",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Processs",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    memory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    family = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    openFiles = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    deviceId = table.Column<int>(type: "int", nullable: false),
                    ArasakaDeviceid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Processs", x => x.id);
                    table.ForeignKey(
                        name: "FK_Processs_Devices_ArasakaDeviceid",
                        column: x => x.ArasakaDeviceid,
                        principalTable: "Devices",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AthenaDataEvents_ArasakaDeviceid",
                table: "AthenaDataEvents",
                column: "ArasakaDeviceid");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_ArasakaClusterid",
                table: "Devices",
                column: "ArasakaClusterid");

            migrationBuilder.CreateIndex(
                name: "IX_MemoryMappings_ArasakaDeviceid",
                table: "MemoryMappings",
                column: "ArasakaDeviceid");

            migrationBuilder.CreateIndex(
                name: "IX_Processs_ArasakaDeviceid",
                table: "Processs",
                column: "ArasakaDeviceid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AthenaDataEvents");

            migrationBuilder.DropTable(
                name: "MemoryMappings");

            migrationBuilder.DropTable(
                name: "Processs");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "ArasakaClusters");
        }
    }
}
