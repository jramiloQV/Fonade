CREATE TABLE [dbo].[InformeBimensual] (
    [id_informeBimensual]      INT            IDENTITY (1, 1) NOT NULL,
    [NomInformeBimensual]      VARCHAR (255)  NOT NULL,
    [codinterventor]           INT            NULL,
    [codempresa]               INT            NULL,
    [Estado]                   INT            NULL,
    [Periodo]                  INT            NULL,
    [Fecha]                    DATETIME       NULL,
    [observacionescoordinador] VARCHAR (1000) NULL,
    CONSTRAINT [PK_InformeBimensual] PRIMARY KEY CLUSTERED ([id_informeBimensual] ASC) WITH (FILLFACTOR = 50)
);

