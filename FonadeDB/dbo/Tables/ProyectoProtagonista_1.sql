CREATE TABLE [dbo].[ProyectoProtagonista] (
    [IdProtagonista]                     INT           IDENTITY (1, 1) NOT NULL,
    [IdProyecto]                         INT           NOT NULL,
    [PerfilesDiferentes]                 BIT           NOT NULL,
    [PerfilConsumidor]                   VARCHAR (MAX) NULL,
    [NecesidadesPotencialesClientes]     VARCHAR (MAX) NOT NULL,
    [NecesidadesPotencialesConsumidores] VARCHAR (MAX) NULL,
    CONSTRAINT [PK_ProyectoProtagonista] PRIMARY KEY CLUSTERED ([IdProtagonista] ASC),
    CONSTRAINT [FK_ProyectoProtagonista_Proyecto] FOREIGN KEY ([IdProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Su proyecto tiene perfiles diferentes de clientes y consumidores?', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ProyectoProtagonista', @level2type = N'COLUMN', @level2name = N'PerfilesDiferentes';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'¿Cuales son las necesidades que usted espera satisfacer de sus potenciales clientes/consumidores?', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ProyectoProtagonista', @level2type = N'COLUMN', @level2name = N'NecesidadesPotencialesConsumidores';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'¿Cuales son las necesidades que usted espera satisfacer de sus potenciales clientes/consumidores?', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ProyectoProtagonista', @level2type = N'COLUMN', @level2name = N'NecesidadesPotencialesClientes';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codigo del proyecto', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ProyectoProtagonista', @level2type = N'COLUMN', @level2name = N'IdProyecto';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Llave primaria autonumerica', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ProyectoProtagonista', @level2type = N'COLUMN', @level2name = N'IdProtagonista';

