<template>
	<view class="wrap">
		<view class="top"></view>
		<view class="content">
			<view class="title">欢迎注册 Sinsen' App</view>
			<u-form :model="registerInput" ref="uForm" label-width="200">
				<u-form-item label="用户名" prop="userName">
					<u-input type="text" v-model="registerInput.userName" placeholder="请输入用户名" />
				</u-form-item>
				<u-form-item label="Email" prop="emailAddress">
					<u-input type="email" v-model="registerInput.emailAddress" placeholder="请输入 Email 地址" />
				</u-form-item>
				<u-form-item label="密码" prop="password">
					<u-input type="password" v-model="registerInput.password" placeholder="请输入密码" />
				</u-form-item>

			</u-form>
			<button @tap="submit" :style="[inputStyle]" class="getCaptcha">注册</button>
			<view class="alternative">
				<view class="password" @tap="toLogin">返回登录</view>
				<view class="issue">遇到问题</view>
			</view>
		</view>
		<view class="buttom">
			<view class="loginType">
			</view>
			<view class="hint">
				注册代表同意
				<text class="link" v-on:click="showAgreement">SinsensApp 用户协议、隐私政策</text>
			</view>
		</view>
	</view>
</template>

<script>
	import {
		requestPost
	} from '@/api/service-base'
	export default {
		data() {
			return {
				registerInput: {
					userName: '',
					emailAddress: '',
					password: '',
					appName: 'SinsensApp'
				},
				rules: {
					userName: {
						required: true,
						message: '请输入用户名',
						tirgger: ['blur', 'change']
					},
					emailAddress: [{
						type: 'email',
						required: true,
						message: '请输入 Email 地址',
						tirgger: ['blur', 'change']
					}],
					password: [{
						required: true,
						message: '请输入密码',
						tirgger: ['blur', 'change']
					}, {
						min: 6,
						max: 16,
						message: '密码长度应在 6-16 个字符'
					}],
				}
			}
		},
		computed: {
			inputStyle() {
				let style = {};
				if (this.tel) {
					style.color = "#fff";
					style.backgroundColor = this.$u.color['warning'];
				}
				return style;
			}
		},
		onReady() {
			uni.setStorage({
				key: 'tenantId',
				data: 1
			})
			this.$refs.uForm.setRules(this.rules);
		},
		methods: {
			toLogin() {
				this.$u.route({
					url: 'pages/login/index'
				})
			},
			showAgreement() {
				uni.showModal({
					title: 'Sinsen\' App 用户协议、隐私政策',
					content: `正在编正在编正在编正在编正在编正在编正在编正在编正在编正在编正在编正在编正在编正,
					在编正在编正在编正在编正在编正在编正在编正在编正在编正在编正在编正在编正在编正在编正在编正在编`,
					showCancel: false
				})
			},
			submit() {
				this.$refs.uForm.validate(valid => {
					if (valid) {
						console.log('验证通过', this.registerInput)
						requestPost({
							url: '/api/account/register',
							data: this.registerInput
						}).then(result => {
							console.log(result)
							if (result.data && result.data.userName)
								uni.showModal({
									title: '提示',
									content: '注册成功！',
									showCancel: false,
									success(r) {
										uni.navigateTo({
											url: '/pages/login/index'
										})
									}
								})
						})
					} else {
						console.log('验证失败');
						return;
					}
				});
			}
		}
	};
</script>

<style lang="scss" scoped>
	.wrap {
		font-size: 28rpx;

		.content {
			width: 600rpx;
			margin: 80rpx auto 0;

			.title {
				text-align: left;
				font-size: 60rpx;
				font-weight: 500;
				margin-bottom: 100rpx;
			}

			input {
				text-align: left;
				margin-bottom: 10rpx;
				padding-bottom: 6rpx;
			}

			.tips {
				color: $u-type-info;
				margin-bottom: 60rpx;
				margin-top: 8rpx;
			}

			.getCaptcha {
				background-color: rgb(253, 243, 208);
				color: $u-tips-color;
				border: none;
				font-size: 30rpx;
				padding: 12rpx 0;

				&::after {
					border: none;
				}
			}

			.alternative {
				color: $u-tips-color;
				display: flex;
				justify-content: space-between;
				margin-top: 30rpx;
			}
		}

		.buttom {
			.loginType {
				display: flex;
				padding: 350rpx 150rpx 150rpx 150rpx;
				justify-content: space-between;

				.item {
					display: flex;
					flex-direction: column;
					align-items: center;
					color: $u-content-color;
					font-size: 28rpx;
				}
			}

			.hint {
				padding: 20rpx 40rpx;
				font-size: 20rpx;
				color: $u-tips-color;

				.link {
					color: $u-type-warning;
				}
			}
		}
	}
</style>
