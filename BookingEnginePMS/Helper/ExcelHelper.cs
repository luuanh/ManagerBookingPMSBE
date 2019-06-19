using BookingEnginePMS.Areas.Admin.Controllers;
using BookingEnginePMS.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace BookingEnginePMS.Helper
{
    public class ExcelHelper
    {
        private string[] sources = {"Walk-in / Telephone","Online Booking Engine","Booking.com","Expedia","Agoda","Trip Connect","AirBNB","Hostelworld","Myallocator","Company","Guest Member","Owner","Returning Guest","Apartment","Siteminder","Other Travel Agency"};
        private string[] status = { "New", "In House", "Check Out", "No Show", "Cancel", "Transfer of Debt" };
        private string[] statusCash = { "Close", "Open" };
        private string[] typeReservation = { "Day", "Hour" };
        private string GetSourceReservation(int index)
        {
            return sources[index - 1];
        }
        private string GetStatus(int index)
        {
            return status[index - 1];
        }
        private string GetTypeReservation(int index)
        {
            return typeReservation[index];
        }
        private string GetStatusCash(bool status)
        {
            return status ? statusCash[1] : statusCash[0];
        }


        public bool Write(string titlefile, List<Booking_Reservation> datas, List<string> titles, string path)
        {
            int width = titles.Count;
            int height = datas.Count;
            FileInfo workbook = new FileInfo(path);
            ExcelPackage myPackage = new ExcelPackage(workbook);
            ExcelWorksheet mySheet = myPackage.Workbook.Worksheets[1];
            mySheet.Cells[1, 1].Value = titlefile;
            mySheet.Cells[2, 1].Value = "Ngày in: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

            object[,] arrData = new object[height + 2, width];
            for (int i = 0; i < width; i++)
            {
                arrData[0, i] = titles[i];
            }
            for (int i = 0; i < height; i++)
            {
                Booking_Reservation model = datas[i];
                arrData[i + 1, 0] = model.ReservationId;
                arrData[i + 1, 1] = model.BookingId;
                arrData[i + 1, 2] = model.GuestName;
                arrData[i + 1, 3] = GetTypeReservation(model.TypeBooking);
                arrData[i + 1, 4] = model.RoomTypeName;
                arrData[i + 1, 5] = model.RoomId < 0 ? "N/A" : model.RoomCode;
                arrData[i + 1, 6] = model.ArrivalDate.ToString("dd/MM/yyyy HH:mm");
                arrData[i + 1, 7] = (model.TypeBooking == 1 && model.Status != 3 && model.Status != 6) ? "______" : model.DepartureDate.ToString("dd/MM/yyyy HH:mm");
                arrData[i + 1, 8] = Math.Round(model.Total, 2);
                arrData[i + 1, 9] = Math.Round(model.Paid + model.PrePaid);
                arrData[i + 1, 10] = Math.Round(model.Total - (model.Paid + model.PrePaid));
                arrData[i + 1, 11] = GetStatus(model.Status);
                arrData[i + 1, 12] = GetSourceReservation(model.BookingSource);
                arrData[i + 1, 13] = model.UserCreate;
            }


            float totalAmount = datas.Sum(x => x.Total);
            float totalPaid = datas.Sum(x => x.PrePaid + x.Paid);
            arrData[height + 1, 7] = "Tổng";
            arrData[height + 1, 8] = Math.Round(totalAmount, 2);
            arrData[height + 1, 9] = Math.Round(totalPaid, 2);
            arrData[height + 1, 10] = Math.Round(totalAmount - totalPaid, 2);

            mySheet.Cells[3, 1, 4 + height, width].Value = arrData;

            using (var range = mySheet.Cells[3, 1, 4 + height, width])
            {
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            }
            myPackage.Save();
            return true;
        }
        public bool WriteActivity(string titlefile, List<Booking_Reservation> datas, List<string> titles, string path)
        {
            int width = titles.Count;
            int height = datas.Count;
            FileInfo workbook = new FileInfo(path);
            ExcelPackage myPackage = new ExcelPackage(workbook);
            ExcelWorksheet mySheet = myPackage.Workbook.Worksheets[1];
            mySheet.Cells[1, 1].Value = titlefile;
            mySheet.Cells[2, 1].Value = "Ngày in: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

            object[,] arrData = new object[height + 2, width];
            for (int i = 0; i < width; i++)
            {
                arrData[0, i] = titles[i];
            }
            for (int i = 0; i < height; i++)
            {
                Booking_Reservation model = datas[i];
                arrData[i + 1, 0] = model.ReservationId;
                arrData[i + 1, 1] = model.BookingId;
                arrData[i + 1, 2] = model.GuestName;
                arrData[i + 1, 3] = GetTypeReservation(model.TypeBooking);
                arrData[i + 1, 4] = model.RoomTypeName;
                arrData[i + 1, 5] = model.RoomId < 0 ? "N/A" : model.RoomCode;
                arrData[i + 1, 6] = model.ArrivalDate.ToString("dd/MM/yyyy HH:mm");
                arrData[i + 1, 7] = (model.TypeBooking == 1 && model.Status != 3 && model.Status != 6) ? "______" : model.DepartureDate.ToString("dd/MM/yyyy HH:mm");
                arrData[i + 1, 8] = Math.Round(model.Total, 2);
                arrData[i + 1, 9] = Math.Round(model.Paid + model.PrePaid);
                arrData[i + 1, 10] = Math.Round(model.Total - (model.Paid + model.PrePaid));
                arrData[i + 1, 11] = GetSourceReservation(model.BookingSource);
                arrData[i + 1, 12] = model.UserCreate;
            }


            float totalAmount = datas.Sum(x => x.Total);
            float totalPaid = datas.Sum(x => x.PrePaid + x.Paid);
            arrData[height + 1, 7] = "Tổng";
            arrData[height + 1, 8] = Math.Round(totalAmount, 2);
            arrData[height + 1, 9] = Math.Round(totalPaid, 2);
            arrData[height + 1, 10] = Math.Round(totalAmount - totalPaid, 2);

            mySheet.Cells[3, 1, 4 + height, width].Value = arrData;

            using (var range = mySheet.Cells[3, 1, 4 + height, width])
            {
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            }
            myPackage.Save();
            return true;
        }
        public bool WriteInvoiceBreakDown(string titlefile, List<Booking_Reservation> datas, List<string> titles, string path)
        {
            int width = titles.Count;
            int height = datas.Count;
            FileInfo workbook = new FileInfo(path);
            ExcelPackage myPackage = new ExcelPackage(workbook);
            ExcelWorksheet mySheet = myPackage.Workbook.Worksheets[1];
            mySheet.Cells[1, 1].Value = titlefile;
            mySheet.Cells[2, 1].Value = "Ngày in: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

            object[,] arrData = new object[height + 2, width];
            for (int i = 0; i < width; i++)
            {
                arrData[0, i] = titles[i];
            }
            for (int i = 0; i < height; i++)
            {
                Booking_Reservation model = datas[i];
                arrData[i + 1, 0] = model.ReservationId;
                arrData[i + 1, 1] = model.BookingId;
                arrData[i + 1, 2] = model.GuestName;
                arrData[i + 1, 3] = GetTypeReservation(model.TypeBooking);
                arrData[i + 1, 4] = model.RoomTypeName;
                arrData[i + 1, 5] = model.RoomId < 0 ? "N/A" : model.RoomCode;
                arrData[i + 1, 6] = model.ArrivalDate.ToString("dd/MM/yyyy HH:mm");
                arrData[i + 1, 7] = (model.TypeBooking == 1 && model.Status != 3 && model.Status != 6) ? "______" : model.DepartureDate.ToString("dd/MM/yyyy HH:mm");
                arrData[i + 1, 8] = Math.Round(model.Total, 2);
                arrData[i + 1, 9] = Math.Round(model.Paid + model.PrePaid);
                arrData[i + 1, 10] = Math.Round(model.Total - (model.Paid + model.PrePaid));
                arrData[i + 1, 11] = GetStatus(model.Status);
            }


            float totalAmount = datas.Sum(x => x.Total);
            float totalPaid = datas.Sum(x => x.PrePaid + x.Paid);
            arrData[height + 1, 7] = "Tổng";
            arrData[height + 1, 8] = Math.Round(totalAmount, 2);
            arrData[height + 1, 9] = Math.Round(totalPaid, 2);
            arrData[height + 1, 10] = Math.Round(totalAmount - totalPaid, 2);

            mySheet.Cells[3, 1, 4 + height, width].Value = arrData;

            using (var range = mySheet.Cells[3, 1, 4 + height, width])
            {
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            }
            myPackage.Save();
            return true;
        }
        public bool WriteAvailableAccommodations(string titlefile, Report_AvailableAccommodations data, List<string> titles, string path)
        {
            int width = titles.Count + 2;
            int height = data.roomTypes.Count;
            FileInfo workbook = new FileInfo(path);
            ExcelPackage myPackage = new ExcelPackage(workbook);
            ExcelWorksheet mySheet = myPackage.Workbook.Worksheets[1];
            mySheet.Cells[1, 1].Value = titlefile;
            mySheet.Cells[2, 1].Value = "Ngày in: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

            object[,] arrData = new object[height + 2, width];
            for (int i = 2; i < width; i++)
            {
                arrData[0, i] = titles[i - 2];
            }
            arrData[1, 0] = "Chỗ ở";
            arrData[1, 1] = data.TotalRoomOfAllRoomType;
            for (int i = 2; i < width; i++)
            {
                arrData[1, i] = data.totalNumberAvailable[i - 2];
            }
            for (int i = 0; i < height; i++)
            {
                arrData[i + 2, 0] = data.roomTypes[i].RoomTypeName;
                arrData[i + 2, 1] = data.roomTypes[i].TotalRoom;
                for (int j = 2; j < width; j++)
                {
                    arrData[i + 2, j] = data.roomTypes[i].NumberRoomAvailable[j - 2];
                }
            }

            mySheet.Cells[3, 1, 4 + height, width].Value = arrData;

            using (var range = mySheet.Cells[3, 1, 4 + height, width])
            {
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            }
            myPackage.Save();
            return true;
        }
        public bool WriteCashDrawer(string titlefile, List<CashHistory> datas, List<string> titles, string path)
        {
            int width = titles.Count;
            int height = datas.Count;
            FileInfo workbook = new FileInfo(path);
            ExcelPackage myPackage = new ExcelPackage(workbook);
            ExcelWorksheet mySheet = myPackage.Workbook.Worksheets[1];
            mySheet.Cells[1, 1].Value = titlefile;
            mySheet.Cells[2, 1].Value = "Ngày in: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

            object[,] arrData = new object[height + 2, width];
            for (int i = 0; i < width; i++)
            {
                arrData[0, i] = titles[i];
            }
            for (int i = 0; i < height; i++)
            {
                CashHistory model = datas[i];
                arrData[i + 1, 0] = model.Name;
                arrData[i + 1, 1] = model.DateOpened.ToString("dd/MM/yyyy HH:mm");
                arrData[i + 1, 2] = model.Status ? "____________" : model.DateClosed.ToString("dd/MM/yyyy HH:mm");
                arrData[i + 1, 3] = Math.Round(model.StartBalance, 2);
                arrData[i + 1, 4] = Math.Round(model.DrawerBalance, 2);
                arrData[i + 1, 5] = Math.Round(model.CashDrop, 2);
                arrData[i + 1, 6] = model.NoteOpen;
                arrData[i + 1, 7] = model.NoteClose;
                arrData[i + 1, 8] = model.UserSession;
                arrData[i + 1, 9] = GetStatusCash(model.Status);
            }

            float totalStartBalance = datas.Sum(x => x.StartBalance);
            float totalDrawerBalance = datas.Sum(x => x.DrawerBalance);
            float totalCashDrop = datas.Sum(x => x.CashDrop);
            arrData[height + 1, 2] = "Tổng";
            arrData[height + 1, 3] = Math.Round(totalStartBalance, 2);
            arrData[height + 1, 4] = Math.Round(totalDrawerBalance, 2);
            arrData[height + 1, 5] = Math.Round(totalCashDrop, 2);

            mySheet.Cells[3, 1, 4 + height, width].Value = arrData;

            using (var range = mySheet.Cells[3, 1, 4 + height, width])
            {
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            }
            myPackage.Save();
            return true;
        }


    }
}