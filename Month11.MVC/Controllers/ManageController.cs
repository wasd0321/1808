using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Month11.MVC.Models;
using System.Data;
using System.IO;
using NPOI.HSSF.UserModel;

namespace Month11.MVC.Controllers
{
    public class ManageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Add()
        {
            return View();
        }
        //查询数据 显示
        public IActionResult RuKuQuery(int page,int limit, int selcang, int seldanju,string yewu,string hetonhao, string hetonname, string dingdan, string rukudan, string zhidan,string zhuangtai,DateTime? rukudate, DateTime? dateend)
        {            
            string sql = "select * from CangKu join RuKuTable on RuKuTable.CangKuId=CangKu.Cid join DanJuType on RuKuTable.Typeid=DanJuType.Did";
            List<RuKuTable> rukus = DBHelper.GetList<RuKuTable>(sql);
            List<RuKuTable> rukuss = rukus;
            if (yewu!=null&&yewu!="-1")
            {
                rukuss = rukuss.Where(s => s.YeWu.Contains(yewu)).ToList();
            }
            if (string.IsNullOrEmpty(hetonhao) == false)
            {
                rukuss = rukuss.Where(s => s.HeTongHao.Contains(hetonhao)).ToList();
            }
            if (string.IsNullOrEmpty(hetonname) == false)
            {
                rukuss = rukuss.Where(s => s.HeTongName.Contains(hetonname)).ToList();
            }
            if (string.IsNullOrEmpty(dingdan) == false)
            {
                rukuss = rukuss.Where(s => s.CaiGoHao.Contains(dingdan)).ToList();
            }
            if (string.IsNullOrEmpty(rukudan) == false)
            {
                rukuss = rukuss.Where(s => s.RuKuHao.Contains(rukudan)).ToList();
            }
            if (string.IsNullOrEmpty(zhidan) == false)
            {
                rukuss = rukuss.Where(s => s.ZhiDanUser.Contains(zhidan)).ToList();
            }
            if (rukudate!=null)
            {
                rukuss = rukuss.Where(s => s.RuKuDate>rukudate).ToList();
            }
            if (dateend != null)
            {
                rukuss = rukuss.Where(s => s.RuKuDate<dateend).ToList();
            }
            if (selcang!=0&&selcang!=-1)
            {
                rukuss = rukuss.Where(s => s.CangKuId==selcang).ToList();
            }
            if (seldanju !=0&&seldanju!=-1)
            {
                rukuss = rukuss.Where(s => s.Typeid==seldanju).ToList();
            }
            if (zhuangtai!=null&&zhuangtai!="-1")
            {
                rukuss = rukuss.Where(s => s.ZhuangTai.Contains(zhuangtai)).ToList();
            }
            rukuss = rukuss.Skip((page - 1) * limit).Take(limit).ToList();
            return Ok(new {data=rukuss,msg="",code=0,count=rukus.Count });
        }
        //显示明细
        public IActionResult MingXiData(int page,int limit)
        {
            string sql = "select * from RuKuMingXiTable";
            List<RuKuMingXiTable> rukus = DBHelper.GetList<RuKuMingXiTable>(sql);
            int rd = new Random().Next(1,9); 
            return Ok(new {data=rukus,code=0,meg="",suiji=rd });
        }
        //绑定下拉
        public IActionResult CangKuBind()
        {
            string sql = "select * from CangKu";
            List<CangKu> rukus = DBHelper.GetList<CangKu>(sql);
            return Ok(rukus);
        }
        //类型绑定下拉
        public IActionResult DanJuBind()
        {
            string sql = "select * from DanJuType";
            List<DanJuType> rukus = DBHelper.GetList<DanJuType>(sql);
            return Ok(rukus);
        }
        //导出
        public IActionResult RuKuDaoChu()
        {
            string sql = "select * from RuKuTable";
            DataSet rukus = DBHelper.GetDataSet(sql);
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet();
            var rowcount = rukus.Tables[0].Rows.Count;
            var coumlscount = rukus.Tables[0].Columns.Count;
            for (int i = 0; i < rowcount; i++)
            {
                var rows = sheet.CreateRow(i);
                for (int h = 0; h < coumlscount; h++)
                {
                    var columns = rows.CreateCell(h);
                    columns.SetCellValue(Convert.ToString(rukus.Tables[0].Rows[i][h]));
                }
            }
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            var typename = "application/octet-stream";
            var exelname = $"{DateTime.Now.ToString("yyyy/MM/dd/hh/mm/ss")}.xls";
            return File(ms.ToArray(), typename, exelname);
        }
        //添加
        public IActionResult RuKuAddData(RuKuTable ru)
        {
            string sql = $"insert into RuKuTable values('{ru.YeWu}','{ru.CaiGoHao}',{ru.Typeid},'{ru.BuMen}','{ru.CengBenType}','{ru.HeTongHao}','{ru.RuKuHao}','{ru.ShuiLv}','{ru.ZhiDanUser}','{ru.HeTongName}',{ru.CangKuId},'{ru.RuKuDate.ToString("yyyy/MM/dd")}','{ru.ChanPinUser}','144','2','已入库','{ru.BeiZhu}')";
            int h = DBHelper.ExecuteNonQuery(sql);
            return Ok(new {code=h,msg=h>0?"添加成功":"添加失败" });
        }
    }
}
