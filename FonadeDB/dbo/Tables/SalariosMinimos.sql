CREATE TABLE [dbo].[SalariosMinimos] (
    [Id_SalariosMinimos] INT          IDENTITY (1, 1) NOT NULL,
    [AñoSalario]         INT          NOT NULL,
    [SiglaAño]           VARCHAR (20) NOT NULL,
    [SalarioMinimo]      BIGINT       NOT NULL,
    CONSTRAINT [PK_SalariosMinimos] PRIMARY KEY CLUSTERED ([Id_SalariosMinimos] ASC)
);

