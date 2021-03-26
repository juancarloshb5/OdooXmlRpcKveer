﻿using Microsoft.Extensions.Configuration;
using Odoo.Concrete;
using Odoo.Extensions;
using OdooSample.Models;
using OdooSample.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OdooSample
{
    class Program
    {
        // Multi Company Ortamlarda hangi şirket üzerinde işlem yapılacak ise o dikkate alınmalı
        private static int _companyId = 1;

        static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            var rpcConnnectionSettings = new RpcConnectionSetting();
            config.GetSection("OdooConnection").Bind(rpcConnnectionSettings);

            //Connection
            var odooConn = new RpcConnection(rpcConnnectionSettings);

            //ExecutePartnerService(odooConn);

            //var result =  CreatePartnerService(odooConn);
            //if (result > 0)
            //    Console.WriteLine("Successs");

            var result = EditPartner(odooConn);

            //var result = new PartnerService(odooConn).Remove(50);
            Console.ReadLine();

        }

        static void CreatePartnerJuan()
        {
            //SaleOrder
            //WriteSaleOrder(odooConn);
            //var partnerRecord = GetPartner(odooConn);
            //var partner = partnerRecord.ToEntity<Partner>();
            ////var partner = Map.Partner(partnerRecord);
            //var record =  CreatePartner(odooConn, "res.partner", -1);
            //Console.WriteLine(partner.Name);
        }

        static int CreatePartnerService(RpcConnection rpcConnection)
        {
            var leslie = new Partner
            {
                Name = "Leslie Morel",
                Active = true,
                Street = "Calle Rafael",
            };
            var partnerService = new PartnerService(rpcConnection);
            var result = partnerService.Create(leslie);
            return result;

        }

        static void ExecutePartnerService(RpcConnection rpcConnection)
        {
            var partnerService = new PartnerService(rpcConnection);

            var partners = partnerService.SearchAndRead(new RpcFilter().Equal("parent_id",11));
        }

        static bool EditPartner(RpcConnection rpcConnection)
        {
            var partnerService = new PartnerService(rpcConnection);
            var partner = partnerService.Get(46);
            partner.Email = "jhernandez@gmail.com";
            partner.CountryId = 61;
            return partnerService.Write(46, partner);
        }
        static RpcRecord CreatePartner(RpcConnection rpcConnection, string model, int? id)
        {
            var partner = new Partner()
            {
                Name = "Juan Carlos",
                Street = "Calle Sana"
            };

            var recordPartner =  partner.ToRpcRecord(rpcConnection, model, id);
            recordPartner.Save();
            return recordPartner;
        }
        //Sale Order - Oluşturma
        static RpcRecord WriteSaleOrder(RpcConnection conn)
        {
            //Partner
            var partner = CreatePartner(conn);
            var rnd = new Random();

            var orderLine = GetSaleOrderLine(conn);
        
            RpcRecord record = new RpcRecord(conn, "sale.order", -1, new List<RpcField>
            {
                new RpcField{FieldName = "company_id", Value = _companyId },
                new RpcField{FieldName = "currency_id", Value = 31},
                new RpcField{FieldName = "date_order", Value = "2021-01-28"}, // posible
                new RpcField{FieldName = "name", Value = "Örnek Sipariş No:" + rnd.Next(1,10000).ToString()},
                new RpcField{FieldName = "partner_id", Value = partner.Id},
                new RpcField{FieldName = "partner_invoice_id", Value = partner.Id},
                new RpcField{FieldName = "partner_shipping_id", Value = partner.Id},
                new RpcField{FieldName = "picking_policy", Value = "one"},
                new RpcField{FieldName = "pricelist_id", Value = 1},
                new RpcField{FieldName = "warehouse_id", Value = 1},
                new RpcField{FieldName = "state", Value = "sale"}, //Onaylı Sipariş ise
                new RpcField{FieldName = "order_line", Value =  orderLine.ToArray() }
            });

            record.Save();
            return record;
        }


        //Sale Order Satırlarını Oluştur
        static List<object> GetSaleOrderLine(RpcConnection conn)
        {
            var orderLine = new List<object>();

            //Ürün 1
            var product = GetSearchProductByDefaultCode(conn, "FURN_6741");

            RpcRecord record = new RpcRecord(conn, "sale.order.line", -1, new List<RpcField>
            {
                new RpcField{FieldName = "name", Value = product.GetField("name").Value},
                new RpcField{FieldName = "customer_lead", Value = 8},
                new RpcField{FieldName = "price_unit", Value = 12.45},
                new RpcField{FieldName = "product_uom_qty", Value = 5},
                new RpcField{FieldName = "product_id", Value = product.Id},
                //new RpcField{FieldName = "tax_id", Value = product.GetField("taxes_id").Value},
            });
            orderLine.Add(new object[] { 0, 0, record.GetRecord() });


            //Ürün 2
            var product2 = GetSearchProductByDefaultCode(conn, "FURN_5555");

            RpcRecord record2 = new RpcRecord(conn, "sale.order.line", -1, new List<RpcField>
            {
                new RpcField{FieldName = "name", Value = product2.GetField("name").Value},
                new RpcField{FieldName = "customer_lead", Value = 8},
                new RpcField{FieldName = "price_unit", Value = 65.75},
                new RpcField{FieldName = "product_uom_qty", Value = 12},
                new RpcField{FieldName = "product_id", Value = product2.Id},
                //new RpcField{FieldName = "tax_id", Value = product2.GetField("taxes_id").Value},
            });
            orderLine.Add(new object[] { 0, 0, record2.GetRecord() });


            //Ürün 3
            var product3 = GetSearchProductByDefaultCode(conn, "FURN_0789");

            RpcRecord record3 = new RpcRecord(conn, "sale.order.line", -1, new List<RpcField>
            {
                new RpcField{FieldName = "name", Value = product3.GetField("name").Value},
                new RpcField{FieldName = "customer_lead", Value = 3},
                new RpcField{FieldName = "price_unit", Value = 165.75},
                new RpcField{FieldName = "product_uom_qty", Value = 8},
                new RpcField{FieldName = "product_id", Value = product3.Id},
                //new RpcField{FieldName = "tax_id", Value = product3.GetField("taxes_id").Value},
            });
            orderLine.Add(new object[] { 0, 0, record3.GetRecord() });

            return orderLine;
        }

        //Ürün Arama
        static RpcRecord GetSearchProductByDefaultCode(RpcConnection conn, string defaultCode)
        {
            var rpcContext = new RpcContext(conn, "product.product");

            rpcContext
                .RpcFilter
                .Or()
                .Equal("company_id", _companyId)
                .Equal("company_id", false)
                .Equal("default_code", defaultCode);

            rpcContext
                .AddField("id")
                .AddField("name")
                .AddField("taxes_id");

            var data = rpcContext.Execute(true, limit: 1);
            return data.FirstOrDefault();
        }

        static RpcRecord GetPartner(RpcConnection rpcConnection)
        {
            var rpcContext = new RpcContext(rpcConnection, "res.partner");
            var data = rpcContext.Execute(true);
            return data.FirstOrDefault();
        }

        // İş Ortağı Oluşturma
        static RpcRecord CreatePartner(RpcConnection conn)
        {
            // Ref alanından kontak ara
            var rpcContext = new RpcContext(conn, "res.partner");

            rpcContext
                .RpcFilter
                .Or() // burada tanımlı OR operatörü odoo'nun normal kullanımında olduğu gibi önündeki koşulu birbirine bağlar belirtilmez ise AND operatörü gibi çalışır
                .Equal("company_id", _companyId)
                .Equal("company_id", false)
                .Equal("ref", "TO9930914");

            /*
             * Yukarıdaki koşulun açılımı
             * if (company_id == 1 or company_id == false) and ref= "TO9930914")
             */

            rpcContext
                .AddField("id");
            var data = rpcContext.Execute(true, limit: 1);
            var partner =  data.FirstOrDefault();
            if (partner != null)
            {
                return partner;
            }

            //İl
            var stateId = GetCountryStateByName(conn, "İstanbul");

            //res.partner - Create
            RpcRecord record = new RpcRecord(conn, "res.partner", -1, new List<RpcField>
            {
                new RpcField{FieldName = "name", Value = "İsmail Eski"},
                new RpcField{FieldName = "street", Value = "Merkez"},
                new RpcField{FieldName = "street2", Value = "Merkez Mh."},
                new RpcField{FieldName = "state_id", Value = stateId},
                new RpcField{FieldName = "vat", Value = "TR1234567890"}
            });
            record.Save();
            return record;
        }

        //İl Arama
        static int GetCountryStateByName(RpcConnection conn, string stateName)
        {
            var rpcContext = new RpcContext(conn, "res.country.state");

            rpcContext
                .RpcFilter.Equal("name", stateName);

            rpcContext
                .AddField("id");

            var data = rpcContext.Execute(limit: 1);
            return data.FirstOrDefault().Id;

        }

    }
}
