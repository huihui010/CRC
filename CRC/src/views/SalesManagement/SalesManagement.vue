<template>
  <div>
    <!--用户列表卡片-->
    <el-card class="box-card">
      <el-form :inline="true" :model="req" class="demo-form-inline">
        <el-form-item label="组号" label-width='70px'>
          <el-select clearable v-model="req.groupID" placeholder="请选择组号" >
            <el-option
              v-for="item in petsCount"
              :key="item.petCategory"
              :label="item.petCategory"
              :value="item.petCategory">
              <span style="float: left">{{ item.petCategory }}</span>
              <span style="float: right; color: #8492a6; font-size: 13px">
                <span class='el-tag el-tag--success el-tag--mini el-tag--plain'> 共{{ item.categoryCount }}条记录</span>
              </span>
            </el-option>
          </el-select>
        </el-form-item>
        <el-form-item clearable label="销售员编号" label-width='90px'>
          <el-input v-model="req.salesmanID" placeholder="请输入销售员编号"></el-input>
        </el-form-item>
        <el-form-item clearable label="电话号码" label-width='85px'>
          <el-input v-model="req.salesmanPhone" placeholder="请输入销售员电话"></el-input>
        </el-form-item>
        <el-form-item label="销售员姓名" label-width='90px'>
          <el-input clearable v-model="req.salesmanName" placeholder="请输入销售员姓名"></el-input>
        </el-form-item>
        <el-form-item style='margin-left: 14px'>
          <el-button icon="el-icon-refresh" @click="chongzhi">重置</el-button>
          <el-button type="primary" icon='el-icon-search' @click="getSalesman">查询</el-button>
          <el-button type="success" icon='el-icon-plus' @click="showAddSalesmanDialog">添加</el-button>
          <el-button type="warning" icon='el-icon-download'>导出</el-button>
        </el-form-item>
      </el-form>
      <!--      表格-->
      <el-table
        :data="SelesList"
        border
        max-height='400'
        style="width: 100%;" >
        <el-table-column
          prop="salesmanID"
          label="销售员编号"
          width="120">
        </el-table-column>
        <el-table-column
          prop="salesmanName"
          label="销售员姓名"
          width="120">
        </el-table-column>
        <el-table-column
          prop="salesmanPhone"
          label="销售员电话"
          width="180">
        </el-table-column>
        <el-table-column
          prop="groupID"
          label="组号"
          width="100">
        </el-table-column>
        <el-table-column
          prop="salesmanIDcard"
          label="组长"
          width="120">
        </el-table-column>
        <el-table-column
          prop="petImage"
          label="照片"
          width="120">
          <!--          <template slot-scope='scope' >-->
          <!--            <img :src="scope.row.petImage + '?x-oss-process=image/resize,w_150 '">-->
          <!--          </template>-->
          <template slot-scope="scope">
            <img :src="scope.row.petImage + '?x-oss-process=image/resize,w_40 '" @click="previewImg(scope.row.petImage)">
          </template>
        </el-table-column>
        <el-table-column
          label="操作"
          width="300">
          <template slot-scope='scope'>
            <!--修改-->
            <el-button type="primary" size="mini" icon="el-icon-edit" @click="showEditDialog(scope.row.salesmanID)"></el-button>
            <!--删除-->
            <el-button type="danger" size="mini" icon="el-icon-delete" @click="DeleteSalesman(scope.row.salesmanID)"></el-button>
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
        <el-form-item label="销售员姓名" prop="salesmanName">
          <el-input v-model="addForm.salesmanName"></el-input>
        </el-form-item>
        <el-form-item label="销售员电话" prop="salesmanPhone">
          <el-input v-model="addForm.salesmanPhone"></el-input>
        </el-form-item>
        <el-form-item label="组号">
          <el-input v-model="addForm.groupID"></el-input>
        </el-form-item>
        <el-form-item label="职位">
          <el-select v-model="addForm.salesmanIDcard" placeholder="请选择">
            <el-option label="组长" value="组长"></el-option>
            <el-option label="副组长" value="副组长"></el-option>
            <el-option label="组员" value="组员"></el-option>
          </el-select>
        </el-form-item>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button @click="addDialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="addSalesmanData">确 定</el-button>
      </span>
    </el-dialog>
    <el-dialog
      title="修改信息"
      :visible.sync="editDialogVisible"
      width="35%"
      @close="editDialogClosed">
      <el-form :model="editForm" :rules="editFormRules" ref="editFormRef" label-width="100px">
        <el-form-item label="销售员编号">
          <el-input v-model="editForm.salesmanID"></el-input>
        </el-form-item>
        <el-form-item label="销售员姓名">
          <el-input v-model="editForm.salesmanName"></el-input>
        </el-form-item>
        <el-form-item label="销售员电话" prop="salesmanPhone">
          <el-input v-model="editForm.salesmanPhone"></el-input>
        </el-form-item>
        <el-form-item label="职位">
          <el-select v-model="editForm.salesmanIDcard" placeholder="请选择">
            <el-option label="组长" value="组长"></el-option>
            <el-option label="副组长" value="副组长"></el-option>
            <el-option label="组员" value="组员"></el-option>
          </el-select>
        </el-form-item>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button @click="editDialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="editSalesmanData">确 定</el-button>
      </span>
    </el-dialog>
  </div>
</template>

<script>
// import { findPetList } from '@/api/pets'
// import { findCategoryAndCount } from '@/api/pets'
import { GetSalesmanData, InsertSalesman, UpdateSalesman, RemoveSalesman, GetUpdateData } from '@/api/SalesManagement'

export default {
  name: 'SalesManagement',
  data() {
    const checksalesmanID = (rule, value, callback) => {
      if (value.length === null) {
        callback(new Error('请输入销售员编号'))
      } else {
        callback()
      }
    }
    const checksalesmanName = (rule, value, callback) => {
      if (value === null) {
        callback(new Error('请输入销售员姓名'))
      } else {
        callback()
      }
    }
    const checksalesmanPhone = (rule, value, callback) => {
      if (value.length !== 11) {
        callback(new Error('电话号码位数不正确'))
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
        salesmanName: [
          { required: true, message: '请输入销售员姓名', trigger: 'blur' },
          { validator: checksalesmanName, trigger: 'blur' }
        ],
        salesmanPhone: [
          { required: true, message: '请输入手机号', trigger: 'blur' },
          { validator: checksalesmanPhone, trigger: 'blur' }
        ]
      },
      editFormRules: {
        salesmanPhone: [
          { required: true, message: '请输入手机号', trigger: 'blur' },
          { validator: checksalesmanPhone, trigger: 'blur' }
        ]
      },
      // 控制修改用户对话框
      editDialogVisible: false,
      addDialogVisible: false,
      // 用户列表
      SelesList: [],
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
    this.getSalesman()
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
    async getSalesman() {
      GetSalesmanData().then(response => {
        this.SelesList = response.data
      })
    },
    editSalesmanData() {
      this.$refs.editFormRef.validate(valid => {
        if (!valid) return
        // 发起请求
        UpdateSalesman({ salesmanID: this.editForm.salesmanID, salesmanName: this.editForm.salesmanName,
          salesmanPhone: this.editForm.salesmanPhone, groupID: this.editForm.groupID, salesmanIDcard: this.editForm.salesmanIDcard }).then(response => {
          if (response.code !== 200) {
            return this.$message.error('更新信息失败')
          }
          this.editDialogVisible = false
          this.getSalesman()
          this.$message.success('更新成功')
        })
      })
    },
    addSalesmanData() {
      this.$refs.addFormRef.validate(valid => {
        if (!valid) return
        InsertSalesman({ salesmanID: this.addForm.salesmanID, salesmanName: this.addForm.salesmanName,
          salesmanPhone: this.addForm.salesmanPhone, groupID: this.addForm.groupID, salesmanIDcard: this.addForm.salesmanIDcard }).then(response => {
          // if (response.code !== 200) {
          //   return this.$message.error('添加失败')
          // }
          this.addDialogVisible = false
          this.getSalesman()
          this.$message.success('添加成功')
        })
      })
    },
    async showAddSalesmanDialog() {
      this.addDialogVisible = true
    },
    async showEditDialog(id) {
      GetUpdateData({ salesmanID: id }).then(response => {
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
        console.log('id是：' + id)
        RemoveSalesman({ salesmanID: id }).then(response => {
          if (response.code === 200) {
            this.$message({
              type: 'success',
              message: '删除成功!'
            })
            this.getSalesman()
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
