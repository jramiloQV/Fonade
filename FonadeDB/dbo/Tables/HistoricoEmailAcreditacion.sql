CREATE TABLE [dbo].[HistoricoEmailAcreditacion] (
    [Id_HistoricoEmailAcreditacion] INT            IDENTITY (1, 1) NOT NULL,
    [CodProyecto]                   INT            NOT NULL,
    [CodConvocatoria]               INT            NOT NULL,
    [CodContacto]                   INT            NOT NULL,
    [Email]                         VARCHAR (2000) NOT NULL,
    [Fecha]                         DATETIME       NOT NULL,
    [CodContactoEnvia]              INT            NULL,
    [CodEstadoAcreditacion]         INT            NULL,
    CONSTRAINT [PK_HistoricoEmailAcreditacion] PRIMARY KEY CLUSTERED ([Id_HistoricoEmailAcreditacion] ASC) WITH (FILLFACTOR = 50)
);

