<template>
  <div>
    <!--用户列表卡片-->
    <el-card class="box-card">
      <el-form :inline="true" :model="req" class="demo-form-inline">
        <el-form-item clearable label="销售员编号" label-width='120px'>
          <el-input v-model="req.contactName" placeholder="请输入销售员编号"></el-input>
        </el-form-item>
        <el-form-item style='margin-left: 14px'>
          <el-button icon="el-icon-refresh" @click="chongzhi">重置</el-button>
          <el-button type="primary" icon='el-icon-search' @click="getContract">查询</el-button>
          <el-button type="success" icon='el-icon-plus' @click="showAddPlanDialog">添加内容</el-button>
          <el-button type="warning" icon='el-icon-download'>导出</el-button>
        </el-form-item>
      </el-form>
      <el-form >
        <el-form-item style='margin-left: 35px'>
          <el-button @click="resetDateFilter" type="danger" icon="el-icon-delete">批量删除</el-button>
        </el-form-item>
      </el-form >
      <!--      表格-->
      <el-table
        :data="ContractList"
        border
        max-height='400'
        style="width: 100%;" >
        <el-table-column
          type="selection"
          width="55">
        </el-table-column>
        <el-table-column
          fixed
          sortable
          prop="contractID"
          label="合约编号"
          width="110">
        </el-table-column>
        <el-table-column
          sortable
          column-key="date"
          prop="contractCommencementDate"
          label="合约生效日期"
          width="200">
        </el-table-column>
        <el-table-column
          sortable
          column-key="contractFailureTime"
          prop="contractFailureTime"
          label="合约失效日期"
          width="200">
        </el-table-column>
        <el-table-column
          prop="salesmanID"
          label="销售员编号"
          width="140">
        </el-table-column>
        <el-table-column
          prop="contractContent"
          label="合约内容"
          width="330">
        </el-table-column>
        <el-table-column
          fixed="right"
          label="操作"
          width="200">
          <template slot-scope='scope'>
            <!--修改-->
            <el-button type="primary" size="mini" icon="el-icon-edit" @click="showEditDialog(scope.row.contractID)"></el-button>
            <el-button type="danger" size="mini" icon="el-icon-delete" @click="DeleteContract(scope.row.contractID)"></el-button>
          </template>
        </el-table-column>
      </el-table>
      <el-pagination
        style='padding-top: 10px'
        @size-change="handleSizeChange"
        @current-change="handleCurrentChange"
        :current-page="current"
        :page-sizes="[7, 10, 20, 30]"
        :page-size="size"
        layout="total, sizes, prev, pager, next, jumper"
        :total="total">
      </el-pagination>
    </el-card>
    <el-dialog
      title="增加信息"
      :visible.sync="addDialogVisible"
      width="35%"
      @close="addDialogClosed">
      <el-form :model="addForm" :rules="addFormRules" ref="addFormRef" label-width="120px">
        <el-form-item label="销售员编号" prop="salesmanID">
          <el-input v-model="addForm.salesmanID" placeholder="输入销售编号"></el-input>
        </el-form-item>
        <el-form-item label="合同生效日期" prop="contractCommencementDate">
          <div class="block">
            <el-date-picker
              v-model="addForm.contractCommencementDate"
              type="datetime"
              placeholder="选择合同生效日期"
              align="right"
              :picker-options="pickerOptions">
            </el-date-picker>
          </div>
        </el-form-item>
        <el-form-item label="合同失效日期" prop="contractFailureTime">
          <div class="block">
            <el-date-picker
              v-model="addForm.contractFailureTime"
              type="datetime"
              placeholder="选择合同失效日期"
              align="right"
              :picker-options="pickerOptions">
            </el-date-picker>
          </div>
        </el-form-item>
        <el-form-item label="合同内容" prop="contractContent" >
          <el-input type="textarea"  v-model="addForm.contractContent" placeholder="输入合同内容"></el-input>
        </el-form-item>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button @click="addDialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="addContractData">确 定</el-button>
      </span>
    </el-dialog>
    <el-dialog
      title="修改信息"
      :visible.sync="editDialogVisible"
      width="35%"
      @close="editDialogClosed">
      <el-form :model="editForm" :rules="editFormRules" ref="editFormRef" label-width="120px">
        <el-form-item label="合同编号">
          <el-input v-model="editForm.contractID" disabled></el-input>
        </el-form-item>
        <el-form-item label="销售员编号">
          <el-input v-model="editForm.salesmanID" disabled></el-input>
        </el-form-item>
        <el-form-item label="合同生效日期">
          <el-input v-model="editForm.contractCommencementDate" disabled></el-input>
        </el-form-item>
        <el-form-item label="合同失效日期" prop="contractFailureTime">
          <div class="block">
            <el-date-picker
              v-model="editForm.contractFailureTime"
              type="datetime"
              placeholder="选择合同失效日期"
              align="right"
              :picker-options="pickerOptions">
            </el-date-picker>
          </div>
        </el-form-item>
        <el-form-item label="合同内容" prop="contractContent">
          <el-input v-model="editForm.contractContent"></el-input>
        </el-form-item>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button @click="editDialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="editComplainData">确 定</el-button>
      </span>
    </el-dialog>
  </div>
</template>

<script>
// import { findPetList } from '@/api/pets'
// import { findCategoryAndCount } from '@/api/pets'
import { GetContractData, InsertContractData, UpdateContractDataData, RemoveContractDataData, GetUpdateData } from '@/api/ContractManagement'

export default {
  name: 'SalesPerformanceManagement',
  data() {
    const checkclientID = (rule, value, callback) => {
      if (value === null) {
        callback(new Error('请输入客户编号'))
      } else {
        callback()
      }
    }
    const checksalesmanID = (rule, value, callback) => {
      if (value === null) {
        callback(new Error('请输入销售员编号'))
      } else {
        callback()
      }
    }
    const checkcontractCommencementDate = (rule, value, callback) => {
      if (value === null) {
        callback(new Error('请选择合同生效日期'))
      } else {
        callback()
      }
    }
    const checkcontractFailureTime = (rule, value, callback) => {
      if (value === null) {
        callback(new Error('请选择合同失效日期'))
      } else {
        callback()
      }
    }
    const checkcontractContent = (rule, value, callback) => {
      if (value === null) {
        callback(new Error('请输入合同内容描述'))
      } else {
        callback()
      }
    }
    return {
      req: {
        salesmanID: '',
        contactName: '',
        contactPhone: '',
        groupID: '',
        salesmanIDcard: ''
      },
      addFormRules: {
        salesmanID: [
          { required: true, message: '销售员编号', trigger: 'blur' },
          { validator: checksalesmanID, trigger: 'blur' }
        ],
        clientID: [
          { required: true, message: '请输入客户编号', trigger: 'blur' },
          { validator: checkclientID, trigger: 'blur' }
        ],
        contractCommencementDate: [
          { required: true, message: '选择合同生效日期', trigger: 'blur' },
          { validator: checkcontractCommencementDate, trigger: 'blur' }
        ],
        contractFailureTime: [
          { required: true, message: '选择合同失效日期', trigger: 'blur' },
          { validator: checkcontractFailureTime, trigger: 'blur' }
        ],
        contractContent: [
          { required: true, message: '输入合同内容', trigger: 'blur' },
          { validator: checkcontractContent, trigger: 'blur' }
        ]
      },
      editFormRules: {
        contractFailureTime: [
          { required: true, message: '合同失效日期不能为空', trigger: 'blur' },
          { validator: checkcontractFailureTime, trigger: 'blur' }
        ],
        contractContent: [
          { required: true, message: '合同内容不能为空', trigger: 'blur' },
          { validator: checkcontractContent, trigger: 'blur' }
        ]
      },
      // 控制修改用户对话框
      editDialogVisible: false,
      addDialogVisible: false,
      // 列表
      ContractList: [],
      editForm: {},
      addForm: {},
      size: 7,
      total: 10,
      current: 1,
      currentPage4: 4,
      petsCount: [],
      // 控制弹窗的显示与隐藏
      addOrUpdateVisible: false
    }
  },
  created() {
    this.getContract()
  },
  methods: {
    resetDateFilter() {
      this.$refs.filterTable.clearFilter('projectEnd')
    },
    clearFilter() {
      this.$refs.filterTable.clearFilter()
    },
    filterHandler(value, row, column) {
      const property = column['property']
      return row[property] === value
    },
    onSubmit() {
      console.log('submit!')
    },
    // 当每页条数改变
    handleSizeChange(val) {
      this.size = val
      this.getPetList()
    },
    // 页码改变
    handleCurrentChange(val) {
      this.current = val
      this.getSalesman()
    },
    editDialogClosed() {
      this.$refs.editFormRef.resetFields()
    },
    addDialogClosed() {
      this.$refs.addFormRef.resetFields()
    },
    async getContract() {
      GetContractData().then(response => {
        console.log('合约数据合同编号：' + response.data[0].contractID)
        console.log('合约数据合约生效日期：' + response.data[0].contractCommencementDate)
        console.log('合约数据销售员编号：' + response.data[0].salesmanID)
        this.ContractList = response.data
      })
    },
    editComplainData() {
      this.$refs.editFormRef.validate(valid => {
        console.log('要传的计划内容' + this.editForm.projectContent)
        if (!valid) return
        // 发起请求
        UpdateContractDataData({
          contractID: this.editForm.contractID,
          contractCommencementDate: this.editForm.contractCommencementDate,
          contractFailureTime: this.editForm.contractFailureTime,
          salesmanID: this.editForm.salesmanID,
          contractContent: this.editForm.contractContent
        }).then(response => {
          if (response.code !== 200) {
            return this.$message.error('更新信息失败')
          }
          this.editDialogVisible = false
          this.getContract()
          this.$message.success('更新成功')
        })
      })
    },
    addContractData() {
      this.$refs.addFormRef.validate(valid => {
        if (!valid) return
        InsertContractData({
          salesmanID: this.addForm.salesmanID,
          contractCommencementDate: this.addForm.contractCommencementDate,
          contractFailureTime: this.addForm.contractFailureTime,
          contractContent: this.addForm.contractContent
        }).then(response => {
          if (response.code !== 200) {
            return this.$message.error('添加失败')
          } else {
            this.addDialogVisible = false
            this.getContract()
            this.$message.success('添加成功')
          }
        })
      })
    },
    async showAddPlanDialog() {
      this.addDialogVisible = true
    },
    async showEditDialog(id) {
      GetUpdateData({ contractID: id }).then(response => {
        this.editForm = response.data[0]
      })
      this.editDialogVisible = true
    },
    DeleteContract(id) {
      this.$confirm('此操作将删除该销售员信息, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => { // 确定执行then方法)
        RemoveContractDataData({ contractID: id }).then(response => {
          if (response.code === 200) {
            this.$message({
              type: 'success',
              message: '删除成功!'
            })
            this.getContract()
          }
        })
      }
      )
    },
    // async getCategoryAndCount() {
    //   const { data } = await findCategoryAndCount()
    //   this.petsCount = data.data.petsCount
    // },
    // 重置
    chongzhi() {
      this.req.salesmanID = ''
      this.req.salesmanName = ''
      this.req.salesmanPhone = ''
      this.req.groupID = ''
      this.req.salesmanIDcard = ''
      this.getPetList()
    },
    // 按钮点击事件 显示弹窗组件
    show() {
      this.addOrUpdateVisible = true
    },
    previewImg(url) {
      this.$hevueImgPreview(url)
    }
  }
}
</script>

<style scoped>

</style>
