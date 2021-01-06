const path = require('path');
const webpack = require('webpack');
const TerserPlugin = require('terser-webpack-plugin');
const CopyWebpackPlugin = require('copy-webpack-plugin');
const CleanWebpackPlugin = require('clean-webpack-plugin');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const OptimizeCSSAssetsPlugin = require('optimize-css-assets-webpack-plugin');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const SpeedMeasurePlugin = require("speed-measure-webpack-plugin");
const smp = new SpeedMeasurePlugin();

module.exports = smp.wrap((env, argv) => {
	return {
		entry: {
			main: './ClientApp/src/main.js'
		},
		output: {
			path: path.resolve(__dirname, 'wwwroot/dist'),
			filename: argv.mode !== 'production' ? '[name].js' : 'bundle-[chunkhash].js',
			publicPath: "/dist/"
		},
		mode: argv.mode,
		module: {
			rules: [
				{
					test: /\.js$/,
					exclude: /node_modules/,
					loader: 'babel-loader',
					options: {
						cacheDirectory: true
					}
				},
				{
					test: /\.(sa|sc|c)ss$/,
					use: ['style-loader', MiniCssExtractPlugin.loader, 'css-loader', 'fast-sass-loader']
				},
				{
					test: /\.(png|jpe?g|gif|woff|woff2|eot|ttf|svg)$/,
					loader: 'file-loader',
					options: {
						name: '[name].[hash].[ext]',
						outputPath: 'assets/'
					}
				}
			]
		},
		plugins: [
			new webpack.ProvidePlugin({
				$: 'jquery',
				jQuery: 'jquery',
				Waves: 'node-waves',
				_: 'underscore',
				Promise: 'es6-promise',
				moment: 'moment'
			}),
			new MiniCssExtractPlugin({
				filename: argv.mode !== 'production' ? '[name].css' : '[name].[hash].css',
				chunkFilename: argv.mode !== 'production' ? '[id].css' : '[id].[hash].css',
				cssProcessorOptions: {
					safe: true,
					discardComments: {
						removeAll: true
					}
				}
			}),
			new CopyWebpackPlugin([
				{
					from: '**/*',
					to: 'mdb-addons',
					context: path.resolve(__dirname, 'src', 'vendors', 'mdb', 'mdb-addons')
				}
			]),
			new CleanWebpackPlugin('wwwroot/dist', { verbose: false }),
			new HtmlWebpackPlugin({
				template: './ClientApp/_LayoutTemplate.cshtml',
				filename: '../../Views/Shared/_Layout.cshtml', //the output root here is /wwwroot/dist so we ../../      
				inject: false
			})
		],
		optimization: {
			minimizer: [
				new TerserPlugin({
					parallel: true,
					sourceMap: true,
					terserOptions: {
						output: {
							comments: false
						}
					}
				}),
				new OptimizeCSSAssetsPlugin({})
			]
		},
		performance: {
			hints: false
		},
		stats: argv.mode !== 'production' ? 'normal' : 'errors-only'
	};
});
