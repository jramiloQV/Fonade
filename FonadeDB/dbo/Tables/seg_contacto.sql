CREATE TABLE [dbo].[seg_contacto] (
    [id]           INT           IDENTITY (1, 1) NOT NULL,
    [id_contacto]  INT           NULL,
    [id_auditoria] INT           NOT NULL,
    [ip]           VARCHAR (200) NULL,
    CONSTRAINT [PK_seg_contacto] PRIMARY KEY CLUSTERED ([id] ASC)
);

