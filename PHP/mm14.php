<?php
error_reporting(0);
define("BANK_KEY", "81726345");//data里的值不正确
define("TEST_HOST", "http://testzzzd.xmxing.net/");// 短信验证错误次数后 当天锁定
header("Content-Type: text/html; charset=utf-8");
//header("Access-Control-Allow-Origin:*");
require_once('DES.php');
$des = new Crypt_DES();
$des->setKey(BANK_KEY);
// $data['sfzhm']="513101198003313012";
$data['cllx']="02";
//$data['hphm']="AUT403";
//$data['cjh']="373240";
$data['hphm']=$_REQUEST["hphm"];
$data['cjh']=$_REQUEST["cjh"];
$data['dwkey']="0766c5cd695f44388c70ed4c042164f5";
$data['to_ken']="987654321123456789";
$data['sjc']="123456";
$djkey="g4We8oaeg4aegPWeh4Ce9YOLhoqDnIaDnIqLk4qAiYaCiYqE2Nqe8tme2Nme1vWe29ae9djX2dHYnNnYnNHXk9HWidnSidXR";
$data['djkey'] = $djkey;
$data_js = json_encode($data);
$mm = base64_encode($des->encrypt($data_js));
// file_get_contents("http://192.168.0.211:9009/sbsfz.php?data=".$mm);
// exit;
// echo "<pre>";print_r($_SERVER);exit;
// $nn = Post("http://192.168.0.211:9009/sbsfz.php",array("data"=>$mm));
// echo TEST_HOST;exit;
$nn = Post(TEST_HOST."/ccx_cj.php",array("data"=>$mm));
// echo TEST_HOST."/sbsfz.php";
// exit;
// echo "<pre>";print_r($nn);exit;
function Post($url, $post = null) {
	if (is_array($post)) {
		ksort($post);
		$content = http_build_query($post);
		$content_length = strlen($content);
		$options = array(
				'http' => array(
						'method' => 'POST',
						'header' =>
						"Content-type: application/x-www-form-urlencoded\r\n" .
						"Content-length: $content_length\r\n",
						'content' => $content
				)
		);
		return file_get_contents($url, false, stream_context_create($options));
	}
}

//echo $nn;

$m = $des->decrypt(base64_decode($nn));
if($m==""){
	echo $nn;exit;
}else{
	echo $m;
}
?>