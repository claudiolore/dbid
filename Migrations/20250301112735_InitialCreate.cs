using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dbid.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AltriImpianti",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Codice = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipoImpianto = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StatoConservativo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Foto = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DichiarazioneConformita = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DichiarazioneRispondenza = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AltriImpianti", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Complessi",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EdificiIds = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Codice = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Tipologia = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Priorita = table.Column<int>(type: "int", nullable: true),
                    Lotto = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RegioneAnas = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProvinciaAnas = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StradaAnas = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProgressivaChilometrica = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ComuneAnas = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LocalitaAnas = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ViaAnas = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NumeroCivicoAnas = table.Column<int>(type: "int", nullable: true),
                    CapAnas = table.Column<int>(type: "int", nullable: true),
                    AgibilitaRilieviArchitettoniciAnas = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    CausaInagibilitaRilieviArchitettoniciAnas = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Operatore = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataRilievo = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CoordinateGeografiche = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RegioneRilevato = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProvinciaRilevato = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ComuneRilevato = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CapRilevato = table.Column<int>(type: "int", nullable: true),
                    LocalitaRilevato = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IndirizziRilevati = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AgibilitaRilieviArchitettoniciRilevato = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    CausaInagibilitaRilieviArchitettoniciRilevato = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DestinazioneUso = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FotoGenerali = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StruttureComplesso = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Note = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ElaboratoPlanimetrico = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AerofotografiaCartografiaStorica = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EstrattoDiMappaCatastale = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PianiUrbanistici = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RelazioneGeotecnicaDelSito = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CategorieUsoNTC2018 = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DocumentoValutazioneRischi = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DichiarazioneInteresseCulturaleArtistico = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IstanzaAccertamentoSussistenza = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SegnalazioneCertificataAgibilita = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProgettoUltimoInterventoEffettuato = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TitoloProprieta = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StruttureIds = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdraulicoAdduzioneIds = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ScarichiIdriciFognariIds = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ImpiantoClimaAcsIds = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ImpiantiElettriciIds = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AltriImpiantiIds = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DocumentiGeneraliIds = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Complessi", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DocumentiGenerali",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Documenti = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descrizione = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentiGenerali", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "IdraulicoAdduzione",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Codice = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipoImpianto = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Contatore = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StatoConservativo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Foto = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ContrattoFornitura = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DichiarazioneConformita = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DichiarazioneRispondenza = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdraulicoAdduzione", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ImpiantiElettrici",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Codice = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DistribuzioneElettrica = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LocaliServiti = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StatoConservativo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Contatore = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FotoContatore = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FotoGenerale = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ContrattoFornitura = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DichiarazioneConformita = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DichiarazioneRispondenza = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImpiantiElettrici", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ImpiantoClimaAcs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Codice = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipoImpianto = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Generatore = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Emissione = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LocaliServiti = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StatoConservativo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Foto = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DichiarazioneConformita = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DichiarazioneRispondenza = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImpiantoClimaAcs", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Infissi",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Codice = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaterialeTelaio = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SpessoreTelaio = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    TaglioTermico = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NumeroAnte = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Vetro = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Larghezza = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    Altezza = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    SistemaOscuramento = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Inferriate = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    StatoConservativo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Foto = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Infissi", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ScarichiIdriciFognari",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Codice = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipoImpianto = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StatoConservativo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AllaccioInFogna = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Foto = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScarichiIdriciFognari", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Strutture",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Codice = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Posizione = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipologiaStruttura = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SottotipologiaStruttura = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SpessoreLordo = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    FinituraInterna = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FinituraEsterna = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StatoConservativo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SegnalazioneProblemaIds = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Strutture", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ComplessoAltriImpianti",
                columns: table => new
                {
                    AltriImpiantiId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ComplessoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplessoAltriImpianti", x => new { x.AltriImpiantiId, x.ComplessoId });
                    table.ForeignKey(
                        name: "FK_ComplessoAltriImpianti_AltriImpianti_AltriImpiantiId",
                        column: x => x.AltriImpiantiId,
                        principalTable: "AltriImpianti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComplessoAltriImpianti_Complessi_ComplessoId",
                        column: x => x.ComplessoId,
                        principalTable: "Complessi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Edifici",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UnitaImmobiliariIds = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AgibilitaRilieviArchitettoniciRilevato = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    CausaInagibilitaRilieviArchitettoniciRilevato = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DestinazioneUsoPrevalenteRilevata = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LivelliFuoriTerra = table.Column<int>(type: "int", nullable: true),
                    LivelliSeminterrati = table.Column<int>(type: "int", nullable: true),
                    LivelliInterrati = table.Column<int>(type: "int", nullable: true),
                    CorpiScala = table.Column<int>(type: "int", nullable: true),
                    TipologiaEdilizia = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PosizioneRispettoAiFabbricatiCircostanti = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StruttureImmobile = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Note = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sezione = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Foglio = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Particella = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipoCatasto = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RicevutePagamentoTributi = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PianoManutenzioneOpera = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SegnalazioneCertificataAgibilita = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProgettoStrutturale = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CollaudoStatico = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UltimoInterventoFabbricato = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipologiaStrutturale = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RapportoVerificaFunzionamentoAscensori = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TitoloEdilizio = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Prospetto = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sezione_Doc = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Planimetria = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StruttureIds = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InfissiIds = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdraulicoAdduzioneIds = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ScarichiIdriciFognariIds = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ImpiantoClimaAcsIds = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ImpiantiElettriciIds = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AltriImpiantiIds = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DocumentiGeneraliIds = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ComplessoId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Edifici", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Edifici_Complessi_ComplessoId",
                        column: x => x.ComplessoId,
                        principalTable: "Complessi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ComplessoDocumentiGenerali",
                columns: table => new
                {
                    ComplessoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DocumentiGeneraliId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplessoDocumentiGenerali", x => new { x.ComplessoId, x.DocumentiGeneraliId });
                    table.ForeignKey(
                        name: "FK_ComplessoDocumentiGenerali_Complessi_ComplessoId",
                        column: x => x.ComplessoId,
                        principalTable: "Complessi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComplessoDocumentiGenerali_DocumentiGenerali_DocumentiGenera~",
                        column: x => x.DocumentiGeneraliId,
                        principalTable: "DocumentiGenerali",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ComplessoIdraulicoAdduzione",
                columns: table => new
                {
                    ComplessoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdraulicoAdduzioneId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplessoIdraulicoAdduzione", x => new { x.ComplessoId, x.IdraulicoAdduzioneId });
                    table.ForeignKey(
                        name: "FK_ComplessoIdraulicoAdduzione_Complessi_ComplessoId",
                        column: x => x.ComplessoId,
                        principalTable: "Complessi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComplessoIdraulicoAdduzione_IdraulicoAdduzione_IdraulicoAddu~",
                        column: x => x.IdraulicoAdduzioneId,
                        principalTable: "IdraulicoAdduzione",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ComplessoImpiantiElettrici",
                columns: table => new
                {
                    ComplessoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ImpiantiElettriciId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplessoImpiantiElettrici", x => new { x.ComplessoId, x.ImpiantiElettriciId });
                    table.ForeignKey(
                        name: "FK_ComplessoImpiantiElettrici_Complessi_ComplessoId",
                        column: x => x.ComplessoId,
                        principalTable: "Complessi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComplessoImpiantiElettrici_ImpiantiElettrici_ImpiantiElettri~",
                        column: x => x.ImpiantiElettriciId,
                        principalTable: "ImpiantiElettrici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ComplessoImpiantoClimaAcs",
                columns: table => new
                {
                    ComplessoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ImpiantoClimaAcsId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplessoImpiantoClimaAcs", x => new { x.ComplessoId, x.ImpiantoClimaAcsId });
                    table.ForeignKey(
                        name: "FK_ComplessoImpiantoClimaAcs_Complessi_ComplessoId",
                        column: x => x.ComplessoId,
                        principalTable: "Complessi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComplessoImpiantoClimaAcs_ImpiantoClimaAcs_ImpiantoClimaAcsId",
                        column: x => x.ImpiantoClimaAcsId,
                        principalTable: "ImpiantoClimaAcs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ComplessoScarichiIdriciFognari",
                columns: table => new
                {
                    ComplessoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ScarichiIdriciFognariId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplessoScarichiIdriciFognari", x => new { x.ComplessoId, x.ScarichiIdriciFognariId });
                    table.ForeignKey(
                        name: "FK_ComplessoScarichiIdriciFognari_Complessi_ComplessoId",
                        column: x => x.ComplessoId,
                        principalTable: "Complessi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComplessoScarichiIdriciFognari_ScarichiIdriciFognari_Scarich~",
                        column: x => x.ScarichiIdriciFognariId,
                        principalTable: "ScarichiIdriciFognari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ComplessoStrutture",
                columns: table => new
                {
                    ComplessoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    StruttureId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplessoStrutture", x => new { x.ComplessoId, x.StruttureId });
                    table.ForeignKey(
                        name: "FK_ComplessoStrutture_Complessi_ComplessoId",
                        column: x => x.ComplessoId,
                        principalTable: "Complessi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComplessoStrutture_Strutture_StruttureId",
                        column: x => x.StruttureId,
                        principalTable: "Strutture",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SegnalazioneProblema",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Descrizione = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Foto = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StruttureId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SegnalazioneProblema", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SegnalazioneProblema_Strutture_StruttureId",
                        column: x => x.StruttureId,
                        principalTable: "Strutture",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EdificioAltriImpianti",
                columns: table => new
                {
                    AltriImpiantiId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EdificioId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EdificioAltriImpianti", x => new { x.AltriImpiantiId, x.EdificioId });
                    table.ForeignKey(
                        name: "FK_EdificioAltriImpianti_AltriImpianti_AltriImpiantiId",
                        column: x => x.AltriImpiantiId,
                        principalTable: "AltriImpianti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EdificioAltriImpianti_Edifici_EdificioId",
                        column: x => x.EdificioId,
                        principalTable: "Edifici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EdificioDocumentiGenerali",
                columns: table => new
                {
                    DocumentiGeneraliId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EdificioId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EdificioDocumentiGenerali", x => new { x.DocumentiGeneraliId, x.EdificioId });
                    table.ForeignKey(
                        name: "FK_EdificioDocumentiGenerali_DocumentiGenerali_DocumentiGeneral~",
                        column: x => x.DocumentiGeneraliId,
                        principalTable: "DocumentiGenerali",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EdificioDocumentiGenerali_Edifici_EdificioId",
                        column: x => x.EdificioId,
                        principalTable: "Edifici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EdificioIdraulicoAdduzione",
                columns: table => new
                {
                    EdificioId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdraulicoAdduzioneId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EdificioIdraulicoAdduzione", x => new { x.EdificioId, x.IdraulicoAdduzioneId });
                    table.ForeignKey(
                        name: "FK_EdificioIdraulicoAdduzione_Edifici_EdificioId",
                        column: x => x.EdificioId,
                        principalTable: "Edifici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EdificioIdraulicoAdduzione_IdraulicoAdduzione_IdraulicoAdduz~",
                        column: x => x.IdraulicoAdduzioneId,
                        principalTable: "IdraulicoAdduzione",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EdificioImpiantiElettrici",
                columns: table => new
                {
                    EdificioId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ImpiantiElettriciId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EdificioImpiantiElettrici", x => new { x.EdificioId, x.ImpiantiElettriciId });
                    table.ForeignKey(
                        name: "FK_EdificioImpiantiElettrici_Edifici_EdificioId",
                        column: x => x.EdificioId,
                        principalTable: "Edifici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EdificioImpiantiElettrici_ImpiantiElettrici_ImpiantiElettric~",
                        column: x => x.ImpiantiElettriciId,
                        principalTable: "ImpiantiElettrici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EdificioImpiantoClimaAcs",
                columns: table => new
                {
                    EdificioId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ImpiantoClimaAcsId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EdificioImpiantoClimaAcs", x => new { x.EdificioId, x.ImpiantoClimaAcsId });
                    table.ForeignKey(
                        name: "FK_EdificioImpiantoClimaAcs_Edifici_EdificioId",
                        column: x => x.EdificioId,
                        principalTable: "Edifici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EdificioImpiantoClimaAcs_ImpiantoClimaAcs_ImpiantoClimaAcsId",
                        column: x => x.ImpiantoClimaAcsId,
                        principalTable: "ImpiantoClimaAcs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EdificioInfissi",
                columns: table => new
                {
                    EdificioId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    InfissiId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EdificioInfissi", x => new { x.EdificioId, x.InfissiId });
                    table.ForeignKey(
                        name: "FK_EdificioInfissi_Edifici_EdificioId",
                        column: x => x.EdificioId,
                        principalTable: "Edifici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EdificioInfissi_Infissi_InfissiId",
                        column: x => x.InfissiId,
                        principalTable: "Infissi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EdificioScarichiIdriciFognari",
                columns: table => new
                {
                    EdificioId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ScarichiIdriciFognariId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EdificioScarichiIdriciFognari", x => new { x.EdificioId, x.ScarichiIdriciFognariId });
                    table.ForeignKey(
                        name: "FK_EdificioScarichiIdriciFognari_Edifici_EdificioId",
                        column: x => x.EdificioId,
                        principalTable: "Edifici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EdificioScarichiIdriciFognari_ScarichiIdriciFognari_Scarichi~",
                        column: x => x.ScarichiIdriciFognariId,
                        principalTable: "ScarichiIdriciFognari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EdificioStrutture",
                columns: table => new
                {
                    EdificioId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    StruttureId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EdificioStrutture", x => new { x.EdificioId, x.StruttureId });
                    table.ForeignKey(
                        name: "FK_EdificioStrutture_Edifici_EdificioId",
                        column: x => x.EdificioId,
                        principalTable: "Edifici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EdificioStrutture_Strutture_StruttureId",
                        column: x => x.StruttureId,
                        principalTable: "Strutture",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UnitaImmobiliari",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AgibilitaRilieviArchitettoniciRilevato = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    CausaInagibilitaRilieviArchitettoniciRilevato = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Scala = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Piano = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Interno = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StatoOccupazioneImmobile = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StatoOccupazioneImmobileDescrizione = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NomeConduttoreEffettivo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DestinazioneUsoRilevata = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DisegnoSchemaInfissi = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Note = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Subalterno = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CategoriaCatastale = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PlanimetriaCatastale = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VisuraIpotecaria = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VisuraCatastaleStorica = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CertificatoDestinazioneUrbanistica = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AttestazionePrestazioneEnergetica = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TitoloEdilizio = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ContrattoUso = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipoUso = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CategoriaAttivita = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SegnalazioneCertificataAgibilita = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RicevutePagamentoTributi = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RilievoArchitettonico = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InfissiIds = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdraulicoAdduzioneIds = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ScarichiIdriciFognariIds = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ImpiantoClimaAcsIds = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ImpiantiElettriciIds = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AltriImpiantiIds = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DocumentiGeneraliIds = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EdificioId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitaImmobiliari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnitaImmobiliari_Edifici_EdificioId",
                        column: x => x.EdificioId,
                        principalTable: "Edifici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UnitaImmobiliareAltriImpianti",
                columns: table => new
                {
                    AltriImpiantiId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UnitaImmobiliareId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitaImmobiliareAltriImpianti", x => new { x.AltriImpiantiId, x.UnitaImmobiliareId });
                    table.ForeignKey(
                        name: "FK_UnitaImmobiliareAltriImpianti_AltriImpianti_AltriImpiantiId",
                        column: x => x.AltriImpiantiId,
                        principalTable: "AltriImpianti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UnitaImmobiliareAltriImpianti_UnitaImmobiliari_UnitaImmobili~",
                        column: x => x.UnitaImmobiliareId,
                        principalTable: "UnitaImmobiliari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UnitaImmobiliareDocumentiGenerali",
                columns: table => new
                {
                    DocumentiGeneraliId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UnitaImmobiliareId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitaImmobiliareDocumentiGenerali", x => new { x.DocumentiGeneraliId, x.UnitaImmobiliareId });
                    table.ForeignKey(
                        name: "FK_UnitaImmobiliareDocumentiGenerali_DocumentiGenerali_Document~",
                        column: x => x.DocumentiGeneraliId,
                        principalTable: "DocumentiGenerali",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UnitaImmobiliareDocumentiGenerali_UnitaImmobiliari_UnitaImmo~",
                        column: x => x.UnitaImmobiliareId,
                        principalTable: "UnitaImmobiliari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UnitaImmobiliareIdraulicoAdduzione",
                columns: table => new
                {
                    IdraulicoAdduzioneId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UnitaImmobiliareId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitaImmobiliareIdraulicoAdduzione", x => new { x.IdraulicoAdduzioneId, x.UnitaImmobiliareId });
                    table.ForeignKey(
                        name: "FK_UnitaImmobiliareIdraulicoAdduzione_IdraulicoAdduzione_Idraul~",
                        column: x => x.IdraulicoAdduzioneId,
                        principalTable: "IdraulicoAdduzione",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UnitaImmobiliareIdraulicoAdduzione_UnitaImmobiliari_UnitaImm~",
                        column: x => x.UnitaImmobiliareId,
                        principalTable: "UnitaImmobiliari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UnitaImmobiliareImpiantiElettrici",
                columns: table => new
                {
                    ImpiantiElettriciId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UnitaImmobiliareId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitaImmobiliareImpiantiElettrici", x => new { x.ImpiantiElettriciId, x.UnitaImmobiliareId });
                    table.ForeignKey(
                        name: "FK_UnitaImmobiliareImpiantiElettrici_ImpiantiElettrici_Impianti~",
                        column: x => x.ImpiantiElettriciId,
                        principalTable: "ImpiantiElettrici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UnitaImmobiliareImpiantiElettrici_UnitaImmobiliari_UnitaImmo~",
                        column: x => x.UnitaImmobiliareId,
                        principalTable: "UnitaImmobiliari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UnitaImmobiliareImpiantoClimaAcs",
                columns: table => new
                {
                    ImpiantoClimaAcsId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UnitaImmobiliareId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitaImmobiliareImpiantoClimaAcs", x => new { x.ImpiantoClimaAcsId, x.UnitaImmobiliareId });
                    table.ForeignKey(
                        name: "FK_UnitaImmobiliareImpiantoClimaAcs_ImpiantoClimaAcs_ImpiantoCl~",
                        column: x => x.ImpiantoClimaAcsId,
                        principalTable: "ImpiantoClimaAcs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UnitaImmobiliareImpiantoClimaAcs_UnitaImmobiliari_UnitaImmob~",
                        column: x => x.UnitaImmobiliareId,
                        principalTable: "UnitaImmobiliari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UnitaImmobiliareInfissi",
                columns: table => new
                {
                    InfissiId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UnitaImmobiliareId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitaImmobiliareInfissi", x => new { x.InfissiId, x.UnitaImmobiliareId });
                    table.ForeignKey(
                        name: "FK_UnitaImmobiliareInfissi_Infissi_InfissiId",
                        column: x => x.InfissiId,
                        principalTable: "Infissi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UnitaImmobiliareInfissi_UnitaImmobiliari_UnitaImmobiliareId",
                        column: x => x.UnitaImmobiliareId,
                        principalTable: "UnitaImmobiliari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UnitaImmobiliareScarichiIdriciFognari",
                columns: table => new
                {
                    ScarichiIdriciFognariId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UnitaImmobiliareId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitaImmobiliareScarichiIdriciFognari", x => new { x.ScarichiIdriciFognariId, x.UnitaImmobiliareId });
                    table.ForeignKey(
                        name: "FK_UnitaImmobiliareScarichiIdriciFognari_ScarichiIdriciFognari_~",
                        column: x => x.ScarichiIdriciFognariId,
                        principalTable: "ScarichiIdriciFognari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UnitaImmobiliareScarichiIdriciFognari_UnitaImmobiliari_Unita~",
                        column: x => x.UnitaImmobiliareId,
                        principalTable: "UnitaImmobiliari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UnitaImmobiliareStrutture",
                columns: table => new
                {
                    StruttureId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UnitaImmobiliareId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitaImmobiliareStrutture", x => new { x.StruttureId, x.UnitaImmobiliareId });
                    table.ForeignKey(
                        name: "FK_UnitaImmobiliareStrutture_Strutture_StruttureId",
                        column: x => x.StruttureId,
                        principalTable: "Strutture",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UnitaImmobiliareStrutture_UnitaImmobiliari_UnitaImmobiliareId",
                        column: x => x.UnitaImmobiliareId,
                        principalTable: "UnitaImmobiliari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ComplessoAltriImpianti_ComplessoId",
                table: "ComplessoAltriImpianti",
                column: "ComplessoId");

            migrationBuilder.CreateIndex(
                name: "IX_ComplessoDocumentiGenerali_DocumentiGeneraliId",
                table: "ComplessoDocumentiGenerali",
                column: "DocumentiGeneraliId");

            migrationBuilder.CreateIndex(
                name: "IX_ComplessoIdraulicoAdduzione_IdraulicoAdduzioneId",
                table: "ComplessoIdraulicoAdduzione",
                column: "IdraulicoAdduzioneId");

            migrationBuilder.CreateIndex(
                name: "IX_ComplessoImpiantiElettrici_ImpiantiElettriciId",
                table: "ComplessoImpiantiElettrici",
                column: "ImpiantiElettriciId");

            migrationBuilder.CreateIndex(
                name: "IX_ComplessoImpiantoClimaAcs_ImpiantoClimaAcsId",
                table: "ComplessoImpiantoClimaAcs",
                column: "ImpiantoClimaAcsId");

            migrationBuilder.CreateIndex(
                name: "IX_ComplessoScarichiIdriciFognari_ScarichiIdriciFognariId",
                table: "ComplessoScarichiIdriciFognari",
                column: "ScarichiIdriciFognariId");

            migrationBuilder.CreateIndex(
                name: "IX_ComplessoStrutture_StruttureId",
                table: "ComplessoStrutture",
                column: "StruttureId");

            migrationBuilder.CreateIndex(
                name: "IX_Edifici_ComplessoId",
                table: "Edifici",
                column: "ComplessoId");

            migrationBuilder.CreateIndex(
                name: "IX_EdificioAltriImpianti_EdificioId",
                table: "EdificioAltriImpianti",
                column: "EdificioId");

            migrationBuilder.CreateIndex(
                name: "IX_EdificioDocumentiGenerali_EdificioId",
                table: "EdificioDocumentiGenerali",
                column: "EdificioId");

            migrationBuilder.CreateIndex(
                name: "IX_EdificioIdraulicoAdduzione_IdraulicoAdduzioneId",
                table: "EdificioIdraulicoAdduzione",
                column: "IdraulicoAdduzioneId");

            migrationBuilder.CreateIndex(
                name: "IX_EdificioImpiantiElettrici_ImpiantiElettriciId",
                table: "EdificioImpiantiElettrici",
                column: "ImpiantiElettriciId");

            migrationBuilder.CreateIndex(
                name: "IX_EdificioImpiantoClimaAcs_ImpiantoClimaAcsId",
                table: "EdificioImpiantoClimaAcs",
                column: "ImpiantoClimaAcsId");

            migrationBuilder.CreateIndex(
                name: "IX_EdificioInfissi_InfissiId",
                table: "EdificioInfissi",
                column: "InfissiId");

            migrationBuilder.CreateIndex(
                name: "IX_EdificioScarichiIdriciFognari_ScarichiIdriciFognariId",
                table: "EdificioScarichiIdriciFognari",
                column: "ScarichiIdriciFognariId");

            migrationBuilder.CreateIndex(
                name: "IX_EdificioStrutture_StruttureId",
                table: "EdificioStrutture",
                column: "StruttureId");

            migrationBuilder.CreateIndex(
                name: "IX_SegnalazioneProblema_StruttureId",
                table: "SegnalazioneProblema",
                column: "StruttureId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitaImmobiliareAltriImpianti_UnitaImmobiliareId",
                table: "UnitaImmobiliareAltriImpianti",
                column: "UnitaImmobiliareId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitaImmobiliareDocumentiGenerali_UnitaImmobiliareId",
                table: "UnitaImmobiliareDocumentiGenerali",
                column: "UnitaImmobiliareId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitaImmobiliareIdraulicoAdduzione_UnitaImmobiliareId",
                table: "UnitaImmobiliareIdraulicoAdduzione",
                column: "UnitaImmobiliareId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitaImmobiliareImpiantiElettrici_UnitaImmobiliareId",
                table: "UnitaImmobiliareImpiantiElettrici",
                column: "UnitaImmobiliareId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitaImmobiliareImpiantoClimaAcs_UnitaImmobiliareId",
                table: "UnitaImmobiliareImpiantoClimaAcs",
                column: "UnitaImmobiliareId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitaImmobiliareInfissi_UnitaImmobiliareId",
                table: "UnitaImmobiliareInfissi",
                column: "UnitaImmobiliareId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitaImmobiliareScarichiIdriciFognari_UnitaImmobiliareId",
                table: "UnitaImmobiliareScarichiIdriciFognari",
                column: "UnitaImmobiliareId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitaImmobiliareStrutture_UnitaImmobiliareId",
                table: "UnitaImmobiliareStrutture",
                column: "UnitaImmobiliareId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitaImmobiliari_EdificioId",
                table: "UnitaImmobiliari",
                column: "EdificioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComplessoAltriImpianti");

            migrationBuilder.DropTable(
                name: "ComplessoDocumentiGenerali");

            migrationBuilder.DropTable(
                name: "ComplessoIdraulicoAdduzione");

            migrationBuilder.DropTable(
                name: "ComplessoImpiantiElettrici");

            migrationBuilder.DropTable(
                name: "ComplessoImpiantoClimaAcs");

            migrationBuilder.DropTable(
                name: "ComplessoScarichiIdriciFognari");

            migrationBuilder.DropTable(
                name: "ComplessoStrutture");

            migrationBuilder.DropTable(
                name: "EdificioAltriImpianti");

            migrationBuilder.DropTable(
                name: "EdificioDocumentiGenerali");

            migrationBuilder.DropTable(
                name: "EdificioIdraulicoAdduzione");

            migrationBuilder.DropTable(
                name: "EdificioImpiantiElettrici");

            migrationBuilder.DropTable(
                name: "EdificioImpiantoClimaAcs");

            migrationBuilder.DropTable(
                name: "EdificioInfissi");

            migrationBuilder.DropTable(
                name: "EdificioScarichiIdriciFognari");

            migrationBuilder.DropTable(
                name: "EdificioStrutture");

            migrationBuilder.DropTable(
                name: "SegnalazioneProblema");

            migrationBuilder.DropTable(
                name: "UnitaImmobiliareAltriImpianti");

            migrationBuilder.DropTable(
                name: "UnitaImmobiliareDocumentiGenerali");

            migrationBuilder.DropTable(
                name: "UnitaImmobiliareIdraulicoAdduzione");

            migrationBuilder.DropTable(
                name: "UnitaImmobiliareImpiantiElettrici");

            migrationBuilder.DropTable(
                name: "UnitaImmobiliareImpiantoClimaAcs");

            migrationBuilder.DropTable(
                name: "UnitaImmobiliareInfissi");

            migrationBuilder.DropTable(
                name: "UnitaImmobiliareScarichiIdriciFognari");

            migrationBuilder.DropTable(
                name: "UnitaImmobiliareStrutture");

            migrationBuilder.DropTable(
                name: "AltriImpianti");

            migrationBuilder.DropTable(
                name: "DocumentiGenerali");

            migrationBuilder.DropTable(
                name: "IdraulicoAdduzione");

            migrationBuilder.DropTable(
                name: "ImpiantiElettrici");

            migrationBuilder.DropTable(
                name: "ImpiantoClimaAcs");

            migrationBuilder.DropTable(
                name: "Infissi");

            migrationBuilder.DropTable(
                name: "ScarichiIdriciFognari");

            migrationBuilder.DropTable(
                name: "Strutture");

            migrationBuilder.DropTable(
                name: "UnitaImmobiliari");

            migrationBuilder.DropTable(
                name: "Edifici");

            migrationBuilder.DropTable(
                name: "Complessi");
        }
    }
}
