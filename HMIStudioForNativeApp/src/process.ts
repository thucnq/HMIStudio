import child_process = require('@skpm/child_process');

/**
 * 外部プロセスを実行する
 * @param path プロセスへのパス
 * @param args 引数配列
 * @param cwd 実行ディレクトリ
 * @param timeout 強制終了タイムアウト値(0:無限待ち)
 * @returns プロセスが正常終了したらtrue
 */
export function execProcessSync(path: string, args: string[] = [], cwd: string = '~', timeout: number = 0): boolean
{
	const defaultOptions = {
		encoding: 'buffer',
		timeout: timeout,
		maxBuffer: 200 * 1024,
		killSignal: 'SIGTERM',
		cwd: cwd,
		env: null,
		shell: true
	}

	try
	{
		const ret = child_process.execFileSync(path, args, defaultOptions);
		return true;
	}
	catch(err)
	{
		log(err);
		return false;
	}
}
