const moment = require("moment");
const webpack = require('webpack');
const UglifyJsPlugin = require('uglifyjs-webpack-plugin');

/**
 * Function that mutates original webpack config.
 * Supports asynchronous changes when promise is returned.
 *
 * @param {object} config - original webpack config.
 * @param {boolean} isPluginCommand - wether the config is for a plugin command or a resource
 **/
module.exports = function (config, isPluginCommand)
{
	config.optimization =
	{
		minimize: false,
		minimizer: [
			new UglifyJsPlugin({
				sourceMap: false,
				parallel: 4,
				uglifyOptions:
				{
					ecma: 8,
					warnings: false,
					toplevel: true,
					keep_classnames: false,
					keep_fnames: false,
					output: {
						comments: false,
						beautify: false,
					}
				}
			})
		]
	};

	config.plugins.push(
		new webpack.DefinePlugin({
			BUILD_TIME: JSON.stringify(moment().unix()),   //ビルド時刻のEPOCH TIME起算経過秒
			EXPIRE_LIMIT: JSON.stringify(6 * 7 * 24 * 3600)    //ビルド後の起動有効時間(6week)
		})
	);
	// config.node={fs: 'empty'};

	// config.resolve= {
    //     alias: {
	// 		'@skmp/fs' : '../../node_modules/@skpm/fs/index.js',
	// 		'xlsx-populate' : '../../node_modules/xlsx-populate/lib/XlsxPopulate.js'
    //     }
    // };
}
