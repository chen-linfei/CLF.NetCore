using System;
using System.Collections.Generic;
using System.Text;

namespace CLF.Service.DTO.Core
{
    /// <summary>
    /// jquery datatable 分页参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PaginatedBaseDTO<T> : BaseEntityDTO where T : BaseEntityDTO
    {
        public PaginatedBaseDTO(int start, int length, int recordsTotal, int recordsFiltered, List<T> data)
        {
            this.start = start;
            this.length = length;
            this.recordsTotal = recordsTotal;
            this.recordsFiltered = recordsFiltered;
            this.data = data;
        }

        /// <summary>
        /// 记录起始位置，(如果每页10条数据，则依次0,10,20,30......类推
        /// </summary>
        public int start { get; set; }
        /// <summary>
        /// 每页记录数量
        /// </summary>
        public int length { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int recordsTotal { get; set; }
        /// <summary>
        /// 过滤后的记录数，通常等于总的记录数
        /// </summary>
        public int recordsFiltered { get; set; }
        public List<T> data { get; set; }
    }
}
