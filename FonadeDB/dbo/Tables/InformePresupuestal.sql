CREATE TABLE [dbo].[InformePresupuestal] (
    [id_informepresupuestal] INT           IDENTITY (1, 1) NOT NULL,
    [NomInformePresupuestal] VARCHAR (255) NOT NULL,
    [codinterventor]         INT           NULL,
    [codempresa]             INT           NULL,
    [Estado]                 INT           NULL,
    [Periodo]                INT           NULL,
    [Fecha]                  DATETIME      NULL,
    CONSTRAINT [PK_InformePresupuestal] PRIMARY KEY CLUSTERED ([id_informepresupuestal] ASC) WITH (FILLFACTOR = 50)
);

