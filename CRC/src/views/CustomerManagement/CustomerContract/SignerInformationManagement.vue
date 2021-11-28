<template>
  <div>
    <!--用户列表卡片-->
    <el-card class="box-card">
      <el-form :inline="true" :model="req" class="demo-form-inline">
        <el-form-item clearable label="销售员编号" label-width='90px'>
          <el-input v-model="req.signatureID" placeholder="请输入销售员编号"></el-input>
        </el-form-item>
        <el-form-item style='margin-left: 14px'>
          <el-button icon="el-icon-refresh" @click="chongzhi">重置</el-button>
          <el-button type="primary" icon='el-icon-search' @click="getSignture">查询</el-button>
          <el-button type="success" icon='el-icon-plus' @click="showAddSigntureDialog">添加签单信息</el-button>
          <el-button type="warning" icon='el-icon-download'>导出</el-button>
        </el-form-item>
      </el-form>
      <!--      表格-->
      <el-table
        :data="SigntureList"
        border
        max-height='400'
        style="width: 100%;" >
        <el-table-column
          prop="signatureID"
          label="签单编号"
          width="120">
        </el-table-column>
        <el-table-column
          prop="salesmanID"
          label="销售员编号"
          width="120">
        </el-table-column>
        <el-table-column
          prop="signatureName"
          label="签单人姓名"
          width="120">
        </el-table-column>
        <el-table-column
          prop="signaturePhone"
          label="签单人电话"
          width="180">
        </el-table-column>
        <el-table-column
          prop="signaturePicture"
          label="签单人签字"
          width="120">
          <template slot-scope="scope">
            <img :src="scope.row.signaturePicture + '?x-oss-process=image/resize,w_40 '" @click="previewImg(scope.row.signaturePicture)">
          </template>
        </el-table-column>
        <el-table-column
          prop="signatureProfession"
          label="签单人职位"
          width="120">
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
        <el-form-item label="签单人姓名" prop="signatureName">
          <el-input v-model="addForm.signatureName"></el-input>
        </el-form-item>
        <el-form-item label="签单人电话" prop="signaturePhone">
          <el-input v-model="addForm.signaturePhone"></el-input>
        </el-form-item>
        <el-form-item label="签单人签字" prop="signaturePicture">
          <el-upload
            v-model="addForm.signaturePicture"
            action="#"
            list-type="picture-card"
            :auto-upload="false">
            <i slot="default" class="el-icon-plus"></i>
            <div slot="file" slot-scope="{file}">
              <img
                class="el-upload-list__item-thumbnail"
                :src="file.url" alt=""
              >
              <span class="el-upload-list__item-actions">
                <span
                  class="el-upload-list__item-preview"
                  @click="handlePictureCardPreview(file)"
                >
                  <i class="el-icon-zoom-in"></i>
                </span>
                <span
                  v-if="!disabled"
                  class="el-upload-list__item-delete"
                  @click="handleDownload(file)"
                >
                  <i class="el-icon-download"></i>
                </span>
                <span
                  v-if="!disabled"
                  class="el-upload-list__item-delete"
                  @click="handleRemove(file)"
                >
                  <i class="el-icon-delete"></i>
                </span>
              </span>
            </div>
          </el-upload>
          <el-dialog :visible.sync="dialogVisible">
            <img width="100%" :src="dialogImageUrl" alt="">
          </el-dialog>
        </el-form-item>
        <el-form-item label="签单人职业" prop="signatureProfession">
          <el-input v-model="addForm.signatureProfession"></el-input>
        </el-form-item>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button @click="addDialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="addSigntureData">确 定</el-button>
      </span>
    </el-dialog>
  </div>
</template>

<script>
// import { findPetList } from '@/api/pets'
// import { findCategoryAndCount } from '@/api/pets'
import { GetSignatureData, InsertSignatureData } from '@/api/SignerInfoMangement'

export default {
  name: 'SignerInformationManagement',
  data() {
    const checksalesmanID = (rule, value, callback) => {
      if (value.length === null) {
        callback(new Error('请输入客户编号'))
      } else {
        callback()
      }
    }
    const checkSigntureName = (rule, value, callback) => {
      if (value === null) {
        callback(new Error('请输入客户姓名'))
      } else {
        callback()
      }
    }
    const checkSignturePhone = (rule, value, callback) => {
      if (value.length !== 11) {
        callback(new Error('电话号码位数不正确'))
      } else {
        callback()
      }
    }
    return {
      req: {
        signatureID: '',
        salesmanID: '',
        signatureName: '',
        signaturePhone: '',
        signaturePicture: '',
        signatureProfession: ''
      },
      addFormRules: {
        salesmanID: [
          { required: true, message: '请输入签单编号', trigger: 'blur' },
          { validator: checksalesmanID, trigger: 'blur' }
        ],
        signatureName: [
          { required: true, message: '请输入签单姓名', trigger: 'blur' },
          { validator: checkSigntureName, trigger: 'blur' }
        ],
        signaturePhone: [
          { required: true, message: '请输入手机号', trigger: 'blur' },
          { validator: checkSignturePhone, trigger: 'blur' }
        ]
      },
      editFormRules: {
        signaturePhone: [
          { required: true, message: '请输入手机号', trigger: 'blur' },
          { validator: checkSignturePhone, trigger: 'blur' }
        ]
      },
      dialogImageUrl: '',
      dialogVisible: false,
      disabled: false,
      // 控制修改用户对话框
      editDialogVisible: false,
      addDialogVisible: false,
      // 用户列表
      SigntureList: [],
      editForm: {},
      addForm: {},
      size: 7,
      total: 5,
      current: 1,
      currentPage4: 4,
      petsCount: [],
      // 控制弹窗的显示与隐藏
      addOrUpdateVisible: false
    }
  },
  created() {
    this.getSignture()
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
      this.getSignture()
    },
    editDialogClosed() {
      this.$refs.editFormRef.resetFields()
    },
    addDialogClosed() {
      this.$refs.addFormRef.resetFields()
    },
    async getSignture() {
      GetSignatureData().then(response => {
        this.SigntureList = response.data
      })
    },
    handleRemove(file) {
      console.log(file)
    },
    handlePictureCardPreview(file) {
      this.dialogImageUrl = file.url
      this.dialogVisible = true
    },
    handleDownload(file) {
      console.log(file)
    },
    addSigntureData() {
      this.$refs.addFormRef.validate(valid => {
        if (!valid) return
        InsertSignatureData({
          salesmanID: this.addForm.salesmanID,
          signatureName: this.addForm.signatureName,
          signaturePhone: this.addForm.signaturePhone,
          signaturePicture: this.addForm.signaturePicture,
          signatureProfession: this.addForm.signatureProfession
        }).then(response => {
          if (response.code !== 200) {
            return this.$message.error('添加失败')
          }
          this.addDialogVisible = false
          this.getSignture()
          this.$message.success('添加成功')
        })
      })
    },
    async showAddSigntureDialog() {
      this.addDialogVisible = true
    },
    // async showEditDialog(id) {
    //   GetUpdateData({ clientID: id }).then(response => {
    //     this.editForm = response.data[0]
    //   })
    //   this.editDialogVisible = true
    // },
    // DeleteSalesman(id) {
    //   this.$confirm('此操作将删除该用户信息, 是否继续?', '提示', {
    //     confirmButtonText: '确定',
    //     cancelButtonText: '取消',
    //     type: 'warning'
    //   }).then(() => { // 确定执行then方法)
    //     console.log('id是：' + id)
    //     RemoveClientData({ clientID: id }).then(response => {
    //       if (response.code === 200) {
    //         this.$message({
    //           type: 'success',
    //           message: '删除成功!'
    //         })
    //         this.getClient()
    //       }
    //     })
    //   }
    //   )
    // },
    // async getCategoryAndCount() {
    //   const { data } = await findCategoryAndCount()
    //   this.petsCount = data.data.petsCount
    // },
    // 重置
    chongzhi() {
      this.req.signatureID = ''
      this.req.signatureName = ''
      this.req.signaturePhone = ''
      this.req.signaturePicture = ''
      this.req.signatureHeadOortrait = ''
      this.req.signatureProfession = ''
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
