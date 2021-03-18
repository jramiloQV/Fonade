CREATE TABLE [dbo].[copia_EvaluacionProyectoIndicador] (
    [id_Indicador]    INT           IDENTITY (1, 1) NOT NULL,
    [codProyecto]     INT           NOT NULL,
    [codConvocatoria] INT           NOT NULL,
    [Descripcion]     VARCHAR (255) NOT NULL,
    [Tipo]            CHAR (1)      NOT NULL,
    [Valor]           FLOAT (53)    NOT NULL,
    [Protegido]       BIT           NOT NULL
);

