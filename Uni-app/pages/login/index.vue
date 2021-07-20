<template>
	<view class="wrap">
		<view class="top"></view>
		<view class="content">
			<view class="title">欢迎登录 Sinsen' App</view>
			<u-form :model="model" ref="uForm">
				<u-form-item label="用户" prop="userNameOrEmailAddress">
					<u-input type="text" v-model="model.userNameOrEmailAddress" placeholder="用户名或 Email" />
				</u-form-item>
				<u-form-item label="密码" prop="password">
					<u-input type="password" v-model="model.password" placeholder="请输入密码" />
				</u-form-item>

			</u-form>
			<button @tap="submit" :style="[inputStyle]" class="getCaptcha">登录</button>
			<view class="alternative">
				<view class="password" @tap="toRegister">立即注册</view>
				<view class="issue">遇到问题</view>
			</view>
		</view>
		<view class="buttom">
			<view class="loginType">
				<!--view class="wechat item">
					<view class="icon">
						<u-icon size="70" name="weixin-fill" color="rgb(83,194,64)"></u-icon>
					</view>
					微信
				</view>
				<view class="QQ item">
					<view class="icon">
						<u-icon size="70" name="qq-fill" color="rgb(17,183,233)"></u-icon>
					</view>
					QQ
				</view-->
			</view>
			<view class="hint">
				登录代表同意
				<text class="link">美团点评用户协议、隐私政策，</text>
				并授权使用您的账号信息（如昵称、头像、收获地址）以便您统一管理
			</view>
		</view>
	</view>
</template>

<script>
	import {
		request
	} from '../../api/service-base'
	import {
		UserLoginInfo
	} from '../../api/service-proxies'
	const clientSetting = {
		grant_type: "password",
		scope: "SinsensApp",
		username: "",
		password: "",
		client_id: "SinsensApp_App",
		client_secret: "1q2w3e*"
	}
	export default {
		data() {
			return {
				model: new UserLoginInfo(),
				ids4info: {},
				rules: {
					userNameOrEmailAddress: [{
						required: true,
						message: '请输入用户名或 Email',
						tirgger: ['blur', 'change']
					}, {
						min: 3,
						max: 32,
						message: '用户名至少 3- 32 个字符长度'
					}],
					password: {
						required: true,
						message: '请输入密码',
						tirgger: ['blur', 'change']
					}
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
			this.$refs.uForm.setRules(this.rules)
			uni.showLoading({
				title: '加载中'
			})
			request({
				url: '/.well-known/openid-configuration'
			}).then(res => {
				console.log('config=>', res)
				uni.hideLoading()
				this.ids4info = res.data
				this.$store.commit('$uStore', {
					name: 'vuex_token_end_points',
					value: res.data
				})
			}).catch(err => {
				uni.showModal({
					title: '加载失败',
					content: '请刷新页面并重试',
					showCancel: false
				})
				console.log(err)
				uni.hideLoading()
			})
		},
		methods: {
			toRegister() {
				this.$u.route({
					url: 'pages/login/register'
				})
			},
			submit() {
				const that = this
				this.$refs.uForm.validate(valid => {
					if (valid) {
						console.log('验证通过');
						uni.showLoading({
							title: '登录中'
						})
						clientSetting.password = this.model.password
						clientSetting.username = this.model.userNameOrEmailAddress
						request({
							url: this.ids4info.token_endpoint,
							method: 'post',
							header: {
								"content-type": "application/x-www-form-urlencoded"
							},
							data: clientSetting,
						}).then(res => {
							uni.hideLoading()
							this.$store.commit('$uStore', {
								name: 'vuex_token',
								value: res.data
							})
							uni.navigateTo({
								url: '/pages/wallets/index'
							})
						}).catch(err => {
							uni.showToast({
								title: err.error_description || err.error || err || '用户名或密码错误',
								icon: 'none'
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
				text-align: center;
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
