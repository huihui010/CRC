<template>
  <div>
    <!--用户列表卡片-->
    <el-card class="box-card">
      <el-form :inline="true" :model="req" class="demo-form-inline">
        <el-form-item clearable label="销售业绩编号" label-width='120px'>
          <el-input v-model="req.performanceID" placeholder="请输入销售业绩编号"></el-input>
        </el-form-item>
        <el-form-item style='margin-left: 14px'>
          <el-button icon="el-icon-refresh" @click="chongzhi">重置</el-button>
          <el-button type="primary" icon='el-icon-search' @click="getSalesman">查询</el-button>
          <el-button type="success" icon='el-icon-plus' @click="showAddPerformanceDialog">添加</el-button>
          <el-button type="warning" icon='el-icon-download'>导出</el-button>
        </el-form-item>
      </el-form>
      <!--      表格-->
      <el-table
        :data="PerformanceList"
        border
        max-height='400'
        style="width: 100%;" >
        <el-table-column
          prop="performanceID"
          label="销售业绩编号"
          width="110">
        </el-table-column>
        <el-table-column
          prop="salesmanID"
          label="销售员编号"
          width="110">
        </el-table-column>
        <el-table-column
          prop="clientID"
          label="客户编号"
          width="150">
        </el-table-column>
        <el-table-column
          prop="contractAmount"
          label="合同金额（￥）"
          width="160">
        </el-table-column>
        <el-table-column
          prop="refundAmount"
          label="回款金额（￥）"
          width="160">
        </el-table-column>
        <el-table-column
          label="操作"
          width="300">
          <template slot-scope='scope'>
            <!--修改-->
            <el-button type="primary" size="mini" icon="el-icon-edit" @click="showEditDialog(scope.row.performanceID)"></el-button>
            <!--删除-->
            <el-button type="danger" size="mini" icon="el-icon-delete" @click="DeleteSalesman(scope.row.performanceID)"></el-button>
            <el-button type="warning" size="mini" icon="el-icon-s-tools"></el-button>
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
      <el-form :model="addForm" :rules="addFormRules" ref="addFormRef" label-width="100px">
        <el-form-item label="销售员编号" prop="salesmanID">
          <el-input v-model="addForm.salesmanID"></el-input>
        </el-form-item>
        <el-form-item label="客户编号" prop="clientID">
          <el-input v-model="addForm.clientID"></el-input>
        </el-form-item>
        <el-form-item label="合同金额" prop="contractAmount">
          <el-input v-model="addForm.contractAmount"></el-input>
        </el-form-item>
        <el-form-item label="回款金额">
          <el-input v-model="addForm.refundAmount"></el-input>
        </el-form-item>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button @click="addDialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="addPerformanceData">确 定</el-button>
      </span>
    </el-dialog>
    <el-dialog
      title="修改信息"
      :visible.sync="editDialogVisible"
      width="35%"
      @close="editDialogClosed">
      <el-form :model="editForm" :rules="editFormRules" ref="editFormRef" label-width="100px">
        <el-form-item label="销售业绩编号">
          <el-input v-model="editForm.performanceID" disabled></el-input>
        </el-form-item>
        <el-form-item label="销售员编号">
          <el-input v-model="editForm.salesmanID" disabled></el-input>
        </el-form-item>
        <el-form-item label="客户编号">
          <el-input v-model="editForm.clientID" disabled></el-input>
        </el-form-item>
        <el-form-item label="合同金额" prop="contractAmount">
          <el-input v-model="editForm.contractAmount"></el-input>
        </el-form-item>
        <el-form-item label="回款金额" prop="refundAmount">
          <el-input v-model="editForm.refundAmount"></el-input>
        </el-form-item>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button @click="editDialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="editPerformanceData">确 定</el-button>
      </span>
    </el-dialog>
  </div>
</template>

<script>
// import { findPetList } from '@/api/pets'
// import { findCategoryAndCount } from '@/api/pets'
import { GetPerformanceData, InsertPerformanceData, UpdatePerformanceData, RemovePerformanceData, GetUpdateData } from '@/api/SalesPerformance'

export default {
  name: 'SalesPerformanceManagement',
  data() {
    const checksalesmanID = (rule, value, callback) => {
      if (value.length === null) {
        callback(new Error('请输入销售员编号'))
      } else {
        callback()
      }
    }
    const checkclientID = (rule, value, callback) => {
      if (value === null) {
        callback(new Error('请输入客户编号'))
      } else {
        callback()
      }
    }
    const checkcontractAmount = (rule, value, callback) => {
      if (value === null) {
        callback(new Error('请输入合同金额'))
      } else {
        callback()
      }
    }
    return {
      req: {
        salesmanID: '',
        salesmanName: '',
        salesmanPhone: '',
        groupID: '',
        salesmanIDcard: ''
      },
      addFormRules: {
        salesmanID: [
          { required: true, message: '请输入销售员编号', trigger: 'blur' },
          { validator: checksalesmanID, trigger: 'blur' }
        ],
        clientID: [
          { required: true, message: '请输入客户编号', trigger: 'blur' },
          { validator: checkclientID, trigger: 'blur' }
        ],
        contractAmount: [
          { required: true, message: '请输入合同金额', trigger: 'blur' },
          { validator: checkcontractAmount, trigger: 'blur' }
        ]
      },
      editFormRules: {
        salesmanID: [
          { required: true, message: '请输入销售员编号', trigger: 'blur' },
          { validator: checksalesmanID, trigger: 'blur' }
        ]
      },
      // 控制修改用户对话框
      editDialogVisible: false,
      addDialogVisible: false,
      // 用户列表
      PerformanceList: [],
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
    this.getPerformance()
    // this.getCategoryAndCount()
  },
  methods: {
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
    async getPerformance() {
      GetPerformanceData().then(response => {
        this.PerformanceList = response.data
      })
    },
    editPerformanceData() {
      console.log('editform :' + this.editForm.performanceID)
      this.$refs.editFormRef.validate(valid => {
        if (!valid) return
        // 发起请求
        UpdatePerformanceData({ performanceID: this.editForm.performanceID, salesmanID: this.editForm.salesmanID,
          clientID: this.editForm.clientID, contractAmount: this.editForm.contractAmount, refundAmount: this.editForm.refundAmount }).then(response => {
          if (response.code !== 200) {
            return this.$message.error('更新信息失败')
          }
          this.editDialogVisible = false
          this.getPerformance()
          this.$message.success('更新成功')
        })
      })
    },
    addPerformanceData() {
      this.$refs.addFormRef.validate(valid => {
        if (!valid) return
        InsertPerformanceData({ salesmanID: this.addForm.salesmanID,
          clientID: this.addForm.clientID, contractAmount: this.addForm.contractAmount, refundAmount: this.addForm.refundAmount }).then(response => {
          if (response.code !== 200) {
            return this.$message.error('添加失败')
          } else {
            this.addDialogVisible = false
            this.getPerformance()
            this.$message.success('添加成功')
          }
        })
      })
    },
    async showAddPerformanceDialog() {
      this.addDialogVisible = true
    },
    async showEditDialog(id) {
      GetUpdateData({ performanceID: id }).then(response => {
        this.editForm = response.data[0]
      })
      this.editDialogVisible = true
    },
    DeleteSalesman(id) {
      this.$confirm('此操作将删除该销售员信息, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => { // 确定执行then方法)
        RemovePerformanceData({ performanceID: id }).then(response => {
          if (response.code === 200) {
            this.$message({
              type: 'success',
              message: '删除成功!'
            })
            this.getPerformance()
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
