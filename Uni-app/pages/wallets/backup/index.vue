<template>
	<view>
		<u-navbar :customBack="back" title="备份还原">
		</u-navbar>
		<u-card title="备份">
			<view slot="body">
				<u-button type="primary" v-if="!backupResult" :loading="loadingStatus.backup" @click="backup">立即生成备份
				</u-button>
				<view v-if="backupResult">
					备份结果
					<u-input type="textarea" v-if="backupResult" v-model="backupResult"></u-input>
					<u-button type="primary" @click="download(backupResult)">下载备份文件</u-button>
					<view class="height-15"></view>
					<u-button @click="copy(backupResult)">复制备份结果</u-button>
				</view>
			</view>
		</u-card>

		<u-card title="还原">
			<view slot="body">
				<view v-if="!resoreReslut">
					<view>还原选项</view>
					<u-checkbox v-model="options.clear">还原前清空数据</u-checkbox>
					<u-checkbox v-if="!options.clear" v-model="options.skip">跳过已有数据（不勾选则覆盖）</u-checkbox>
					<uni-file-picker mode="list" file-mediatype="all" @select="select" file-extname="json" title="备份文件"
						v-model="files" :auto-upload="false" :limit="1"></uni-file-picker>
					<u-button type="warning" @click="doRestore" :loading="loadingStatus.restore">立即还原</u-button>
				</view>
				<view v-if="resoreReslut">
					<u-input type="textarea" v-model="resoreReslut"></u-input>
					<u-button @click="copy(resoreReslut)">复制还原结果</u-button>
				</view>
			</view>
		</u-card>
	</view>
</template>

<script>
	import {
		request,
		getHeader
	} from '@/api/service-base'
	export default {
		data() {
			return {
				files: [],
				backupResult: '',
				resoreReslut: '',
				options: {
					clear: true,
					skip: true
				},
				loadingStatus: {
					restore: false,
					backup: false
				}
			}
		},
		methods: {
			download(url) {
				uni.getSystemInfo({
					success(res) {
						console.log(res)
						if (res.model == 'PC') {
							var form = document.createElement('form')
							form.action = url
							document.getElementsByTagName('body')[0].appendChild(form)
							form.submit()
							uni.showModal({
								title: '提示',
								content: 'H5 暂不支持直接下载',
								showCancel: false
							})
						} else {
							uni.downloadFile({
								url: url,
								success(res) {
									uni.saveFile({
										tempFilePath: res.tempFilePath
									})
								}
							})
						}
					}
				})
			},
			copy(text) {
				uni.setClipboardData({
					data: text || ''
				})
			},
			backup() {
				this.loadingStatus.backup = true
				request({
					url: '/api/app/wallet-backup/backup'
				}).then(res => {
					this.backupResult = res.data.url
					this.loadingStatus.backup = false
				}).catch(err => {
					this.loadingStatus.backup = false
				})

			},
			select(e) {
				console.log(this.files)
				this.files = e.tempFiles
				console.log(e)
			},
			doRestore() {
				if (this.files.length < 1) {
					uni.showToast({
						title: '请选择备份文件',
						icon: 'none'
					})
					return
				}
				this.loadingStatus.restore = true
				const {
					clear,
					skip
				} = this.options

				const that = this
				const option = getHeader({
					url: `/api/app/wallet-backup/restore?clearBeforeRestore=${clear}&skipIfExists=${skip}`,
					filePath: this.files[0].path,
					name: this.files[0].name,
					success(res) {
						that.resoreReslut = res.data || res
					},
					complete() {
						that.loadingStatus.restore = false
					}
				})

				uni.uploadFile(option)
			},
			back() {
				uni.reLaunch({
					url: '/pages/wallets/index'
				})
			}
		}
	}
</script>

<style>

</style>
