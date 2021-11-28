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
          <el-button type="primary" icon='el-icon-search' @click="getAction">查询</el-button>
          <el-button type="success" icon='el-icon-plus' @click="showAddPlanDialog">添加行动记录</el-button>
          <el-button type="warning" icon='el-icon-download'>导出</el-button>
        </el-form-item>
      </el-form>
      <!--      表格-->
      <el-table
        :data="ActionList"
        border
        max-height='400'
        style="width: 100%;" >
        <el-table-column
          type="selection"
          width="55">
        </el-table-column>
        <el-table-column
          sortable
          prop="actionID"
          label="行动记录编号"
          width="130">
        </el-table-column>
        <el-table-column
          sortable
          prop="salesmanID"
          label="销售员编号"
          width="120">
        </el-table-column>
        <el-table-column
          sortable
          column-key="date"
          prop="actionDate"
          label="行动记录日期"
          width="200">
        </el-table-column>
        <el-table-column
          prop="actionMan"
          label="行动人"
          width="180">
        </el-table-column>
        <el-table-column
          prop="siteRecord"
          label="现场记录"
          width="250">
        </el-table-column>
        <el-table-column
          prop="phoneRecord"
          label="电话记录"
          width="250">
        </el-table-column>
<!--        <el-table-column-->
<!--          label="操作"-->
<!--          width="200">-->
<!--          <template slot-scope='scope'>-->
            <!--修改-->
<!--            <el-button type="primary" size="mini" icon="el-icon-edit" @click="showEditDialog(scope.row.projectID)"></el-button>-->
<!--            <el-button type="danger" size="mini" icon="el-icon-delete" @click="DeletePlan(scope.row.projectID)"></el-button>-->
<!--          </template>-->
<!--        </el-table-column>-->
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
        <el-form-item label="行动人" prop="actionMan">
          <el-input v-model="addForm.actionMan" placeholder="请输入行动人姓名"></el-input>
        </el-form-item>
        <el-form-item label="行动日期" prop="actionDate">
          <div class="block">
            <el-date-picker
              v-model="addForm.actionDate"
              type="datetime"
              placeholder="选择行动日期"
              align="right"
              :picker-options="pickerOptions">
            </el-date-picker>
          </div>
        </el-form-item>
        <el-form-item label="现场记录" prop="siteRecord" >
          <el-input type="textarea" v-model="addForm.siteRecord" placeholder="输入现场记录"></el-input>
        </el-form-item>
        <el-form-item label="电话记录" prop="phoneRecord">
          <el-input type="textarea" v-model="addForm.phoneRecord" placeholder="输入电话记录">></el-input>
        </el-form-item>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button @click="addDialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="addActionData">确 定</el-button>
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
import { GetActionData, InsertActionData } from '@/api/Action'

export default {
  name: 'SalesPerformanceManagement',
  data() {
    const checksalesmanID = (rule, value, callback) => {
      if (value === null) {
        callback(new Error('请输入销售员编号'))
      } else {
        callback()
      }
    }
    const checkactionDate = (rule, value, callback) => {
      if (value === null) {
        callback(new Error('请选择行动日期'))
      } else {
        callback()
      }
    }
    const checkactionMan = (rule, value, callback) => {
      if (value === null) {
        callback(new Error('请输入行动人姓名'))
      } else {
        callback()
      }
    }
    const checksiteRecord = (rule, value, callback) => {
      if (value === null) {
        callback(new Error('请输入现场记录'))
      } else {
        callback()
      }
    }
    const checkphoneRecord = (rule, value, callback) => {
      if (value === null) {
        callback(new Error('请输入电话记录'))
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
        actionDate: [
          { required: true, message: '选择行动日期', trigger: 'blur' },
          { validator: checkactionDate, trigger: 'blur' }
        ],
        actionMan: [
          { required: true, message: '请输入行动人姓名', trigger: 'blur' },
          { validator: checkactionMan, trigger: 'blur' }
        ],
        siteRecord: [
          { required: true, message: '请输入现场记录', trigger: 'blur' },
          { validator: checksiteRecord, trigger: 'blur' }
        ],
        phoneRecord: [
          { required: true, message: '请输入电话记录', trigger: 'blur' },
          { validator: checkphoneRecord, trigger: 'blur' }
        ]
      },
      editFormRules: {
      },
      // 控制修改用户对话框
      editDialogVisible: false,
      addDialogVisible: false,
      // 用户列表
      ActionList: [],
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
    this.getAction()
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
    async getAction() {
      GetActionData().then(response => {
        this.ActionList = response.data
      })
    },
    addActionData() {
      this.$refs.addFormRef.validate(valid => {
        if (!valid) return
        InsertActionData({
          salesmanID: this.addForm.salesmanID,
          actionDate: this.addForm.actionDate,
          actionMan: this.addForm.actionMan,
          siteRecord: this.addForm.siteRecord,
          phoneRecord: this.addForm.phoneRecord
        }).then(response => {
          if (response.code !== 200) {
            return this.$message.error('添加失败')
          } else {
            this.addDialogVisible = false
            this.getAction()
            this.$message.success('添加成功')
          }
        })
      })
    },
    async showAddPlanDialog() {
      this.addDialogVisible = true
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
