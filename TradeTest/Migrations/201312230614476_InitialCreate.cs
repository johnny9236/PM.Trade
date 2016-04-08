namespace TradeTest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.T_HuangShan_AcctDtl",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SectionCode = c.String(),
                        AuthCode = c.String(),
                        QYType = c.String(),
                        InDate = c.String(),
                        InTime = c.String(),
                        InAmount = c.String(),
                        InName = c.String(),
                        InAcct = c.String(),
                        InMemo = c.String(),
                        HstSeqNum = c.String(),
                        PunInst = c.String(),
                        Gernal = c.String(),
                        Result = c.String(),
                        AddWord = c.String(),
                        Match = c.Int(nullable: false),
                        Flag = c.Int(nullable: false),
                        Remark = c.String(),
                        CreateTm = c.DateTime(),
                        UpdateTm = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.T_HuangShan_AcctRtnDtl",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SectionCode = c.String(),
                        AuthCode = c.String(),
                        QYType = c.String(),
                        RetDate = c.String(),
                        RetTime = c.String(),
                        RetAmount = c.String(),
                        RetPunInst = c.String(),
                        RetTotal = c.String(),
                        RetName = c.String(),
                        RetAcct = c.String(),
                        HstSeqNum = c.String(),
                        AcctNo = c.String(),
                        Serial_No = c.String(),
                        Match = c.Int(nullable: false),
                        Flag = c.Int(nullable: false),
                        Remark = c.String(),
                        CreateTm = c.DateTime(),
                        UpdateTm = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.T_BOC",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RspCod = c.String(),
                        RspMsg = c.String(),
                        PayIbkNum = c.String(),
                        PayIbkName = c.String(),
                        PayActacn = c.String(),
                        PayAcntName = c.String(),
                        ReciveToIbkn = c.String(),
                        ReciveActacn = c.String(),
                        ReciveToName = c.String(),
                        ReciveToBank = c.String(),
                        MactIbkn = c.String(),
                        Mactacn = c.String(),
                        Mactname = c.String(),
                        MactBank = c.String(),
                        Vchnum = c.String(),
                        TransId = c.String(),
                        TxnDate = c.String(),
                        TxnTime = c.String(),
                        TxNamt = c.String(),
                        Acctbal = c.String(),
                        Avlbal = c.String(),
                        FrzAmt = c.String(),
                        OverdrAmt = c.String(),
                        AvloverdrAmt = c.String(),
                        UseInfo = c.String(),
                        FurInfo = c.String(),
                        TransType = c.String(),
                        BusType = c.String(),
                        TrnCur = c.String(),
                        Direction = c.String(),
                        FeeAct = c.String(),
                        FeeAmt = c.String(),
                        FeeCur = c.String(),
                        ValDat = c.String(),
                        VouchTp = c.String(),
                        VouchNum = c.String(),
                        FxRate = c.String(),
                        InterInfo = c.String(),
                        Reserve1 = c.String(),
                        Reserve2 = c.String(),
                        Reserve3 = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.T_BOC");
            DropTable("dbo.T_HuangShan_AcctRtnDtl");
            DropTable("dbo.T_HuangShan_AcctDtl");
        }
    }
}
