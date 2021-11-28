<template>
  <div>
    <!--用户列表卡片-->
    <el-card class="box-card">
      <el-form :inline="true" :model="req" class="demo-form-inline">
        <el-form-item clearable label="销售员编号" label-width='120px'>
          <el-input v-model="req.contactName" placeholder="请输入销售员编号"></el-input>
        </el-form-item>
        <el-form-item clearable label="客户编号" label-width='120px'>
          <el-input v-model="req.contactName" placeholder="请输入客户编号"></el-input>
        </el-form-item>
        <el-form-item style='margin-left: 14px'>
          <el-button icon="el-icon-refresh" @click="chongzhi">重置</el-button>
          <el-button type="primary" icon='el-icon-search' @click="getPlan">查询</el-button>
          <el-button type="success" icon='el-icon-plus' @click="showAddPlanDialog">添加计划内容</el-button>
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
        ref="filterTable"
        :data="PlanList"
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
          prop="projectID"
          label="计划编号"
          width="110">
        </el-table-column>
        <el-table-column
          sortable
          prop="salesmanID"
          label="销售员编号"
          width="120">
        </el-table-column>
        <el-table-column
          sortable
          prop="clientID"
          label="客户编号"
          width="110">
        </el-table-column>
        <el-table-column
          sortable
          column-key="date"
          prop="projectBegin"
          label="计划开始日期"
          width="180">
        </el-table-column>
        <el-table-column
          sortable
          column-key="projectEnd"
          prop="projectEnd"
          label="计划结束日期"
          width="180">
        </el-table-column>
        <el-table-column
          prop="projectContent"
          label="计划内容"
          width="200">
        </el-table-column>
        <el-table-column
          prop="projectExpected"
          label="计划预期"
          width="200">
        </el-table-column>
        <el-table-column
          fixed="right"
          label="操作"
          width="200">
          <template slot-scope='scope'>
            <!--修改-->
            <el-button type="primary" size="mini" icon="el-icon-edit" @click="showEditDialog(scope.row.projectID)"></el-button>
            <el-button type="danger" size="mini" icon="el-icon-delete" @click="DeletePlan(scope.row.projectID)"></el-button>
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
        <el-form-item label="客户编号" prop="clientID">
          <el-input v-model="addForm.clientID" placeholder="客户编号"></el-input>
        </el-form-item>
        <el-form-item label="计划开始日期" prop="projectBegin">
          <div class="block">
            <el-date-picker
              v-model="addForm.projectBegin"
              type="datetime"
              placeholder="选择开始日期时间"
              align="right"
              :picker-options="pickerOptions">
            </el-date-picker>
          </div>
        </el-form-item>
        <el-form-item label="计划结束日期" prop="projectEnd">
          <div class="block">
            <el-date-picker
              v-model="addForm.projectEnd"
              type="datetime"
              placeholder="选择结束日期时间"
              align="right"
              :picker-options="pickerOptions">
            </el-date-picker>
          </div>
        </el-form-item>
        <el-form-item label="计划内容" prop="projectContent" >
          <el-input type="textarea"  v-model="addForm.projectContent" placeholder="输入计划内容"></el-input>
        </el-form-item>
        <el-form-item label="预期内容" prop="projectExpected">
          <el-input type="textarea" v-model="addForm.projectExpected" placeholder="输入预期内容">></el-input>
        </el-form-item>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button @click="addDialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="addPlanData">确 定</el-button>
      </span>
    </el-dialog>
    <el-dialog
      title="修改信息"
      :visible.sync="editDialogVisible"
      width="35%"
      @close="editDialogClosed">
      <el-form :model="editForm" :rules="editFormRules" ref="editFormRef" label-width="120px">
        <el-form-item label="计划编号">
          <el-input v-model="editForm.projectID" disabled></el-input>
        </el-form-item>
        <el-form-item label="销售员编号">
          <el-input v-model="editForm.salesmanID" disabled></el-input>
        </el-form-item>
        <el-form-item label="客户编号">
          <el-input v-model="editForm.clientID" disabled></el-input>
        </el-form-item>
        <el-form-item label="计划开始日期" prop="projectBegin">
          <div class="block">
            <el-date-picker
              v-model="editForm.projectBegin"
              type="datetime"
              placeholder="选择开始日期时间"
              align="right"
              :picker-options="pickerOptions">
            </el-date-picker>
          </div>
        </el-form-item>
        <el-form-item label="计划结束日期" prop="projectEnd">
          <div class="block">
            <el-date-picker
              v-model="editForm.projectEnd"
              type="datetime"
              placeholder="选择结束日期时间"
              align="right"
              :picker-options="pickerOptions">
            </el-date-picker>
          </div>
        </el-form-item>
        <el-form-item label="计划内容">
          <el-input v-model="editForm.projectContent"></el-input>
        </el-form-item>
        <el-form-item label="计划预期">
          <el-input v-model="editForm.projectExpected"></el-input>
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
import { GetPlanData, InsertPlanData, UpdatePlanData, RemovePlanData, GetUpdateData } from '@/api/Action'

export default {
  name: 'SalesPerformanceManagement',
  data() {
    const checkcontactPhone = (rule, value, callback) => {
      if (value.length !== 11) {
        callback(new Error('电话号码位数不对'))
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
    const checksalesmanID = (rule, value, callback) => {
      if (value === null) {
        callback(new Error('请输入销售员编号'))
      } else {
        callback()
      }
    }
    const checkprojectBegin = (rule, value, callback) => {
      if (value === null) {
        callback(new Error('请输入处理人姓名'))
      } else {
        callback()
      }
    }
    const checkprojectEnd = (rule, value, callback) => {
      if (value === null) {
        callback(new Error('请输入投诉详情描述'))
      } else {
        callback()
      }
    }
    const checkprojectContent = (rule, value, callback) => {
      if (value === null) {
        callback(new Error('请输入投诉详情描述'))
      } else {
        callback()
      }
    }
    const checkprojectExpected = (rule, value, callback) => {
      if (value === null) {
        callback(new Error('请输入投诉详情描述'))
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
        projectBegin: [
          { required: true, message: '选择开始日期', trigger: 'blur' },
          { validator: checkprojectBegin, trigger: 'blur' }
        ],
        projectEnd: [
          { required: true, message: '选择结束日期', trigger: 'blur' },
          { validator: checkprojectEnd, trigger: 'blur' }
        ],
        projectContent: [
          { required: true, message: '输入计划内容', trigger: 'blur' },
          { validator: checkprojectContent, trigger: 'blur' }
        ],
        projectExpected: [
          { required: true, message: '请输入预期成果', trigger: 'blur' },
          { validator: checkprojectExpected, trigger: 'blur' }
        ]
      },
      editFormRules: {
        contactPhone: [
          { required: true, message: '请输入联系人电话', trigger: 'blur' },
          { validator: checkcontactPhone, trigger: 'blur' }
        ]
      },
      // 控制修改用户对话框
      editDialogVisible: false,
      addDialogVisible: false,
      // 用户列表
      PlanList: [],
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
    this.getPlan()
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
    async getPlan() {
      GetPlanData().then(response => {
        this.PlanList = response.data
      })
    },
    editComplainData() {
      this.$refs.editFormRef.validate(valid => {
        console.log('要传的计划内容' + this.editForm.projectContent)
        if (!valid) return
        // 发起请求
        UpdatePlanData({
          projectID: this.editForm.projectID,
          salesmanID: this.editForm.salesmanID,
          clientID: this.editForm.clientID,
          projectBegin: this.editForm.projectBegin,
          projectEnd: this.editForm.projectEnd,
          projectContent: this.editForm.projectContent,
          projectExpected: this.editForm.projectExpected
        }).then(response => {
          if (response.code !== 200) {
            return this.$message.error('更新信息失败')
          }
          this.editDialogVisible = false
          this.getPlan()
          this.$message.success('更新成功')
        })
      })
    },
    addPlanData() {
      this.$refs.addFormRef.validate(valid => {
        if (!valid) return
        InsertPlanData({
          salesmanID: this.addForm.salesmanID,
          clientID: this.addForm.clientID,
          projectBegin: this.addForm.projectBegin,
          projectEnd: this.addForm.projectEnd,
          projectContent: this.addForm.projectContent,
          projectExpected: this.addForm.projectExpected
        }).then(response => {
          if (response.code !== 200) {
            return this.$message.error('添加失败')
          } else {
            this.addDialogVisible = false
            this.getPlan()
            this.$message.success('添加成功')
          }
        })
      })
    },
    async showAddPlanDialog() {
      this.addDialogVisible = true
    },
    async showEditDialog(id) {
      GetUpdateData({ projectID: id }).then(response => {
        this.editForm = response.data[0]
      })
      this.editDialogVisible = true
    },
    DeletePlan(id) {
      this.$confirm('此操作将删除该销售员信息, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => { // 确定执行then方法)
        RemovePlanData({ projectID: id }).then(response => {
          if (response.code === 200) {
            this.$message({
              type: 'success',
              message: '删除成功!'
            })
            this.getPlan()
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
