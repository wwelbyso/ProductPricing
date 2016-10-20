CREATE TABLE [dbo].[Premium] (
    [premiumID]   INT      NOT NULL,
    [age]         SMALLINT NOT NULL,
    [Premium]     MONEY    NOT NULL,
    [sumDeductID] INT      NOT NULL,
    CONSTRAINT [Pk_Premium] primary key clustered ([sumDeductID] asc),
    CONSTRAINT [FK_Product_Premium] FOREIGN KEY ([sumDeductID]) REFERENCES [SumDeduct] ([sumDeductID])  
);


GO
CREATE NONCLUSTERED INDEX [IX_SumDeduct]
    ON [dbo].[Premium]([sumDeductID] ASC);

