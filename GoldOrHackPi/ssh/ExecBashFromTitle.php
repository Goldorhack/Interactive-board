<?php

require_once('function_ssh.php');

/// phase 01 : recuperer le host selectioné.

$host = "";
$username = "";
$password = "";

$host_get = "";

if (isset($_GET["host"]))
{
    $host_get = $_GET["host"];
}

		
/// phase 02 : recuperer les données de connection

require_once('../config/ssh_serv_list.php');

$tts_ssh = getTtsFromCsvString($ssh_csv_info);

for($i = 0; $i < count($tts_ssh); $i++)
{
    if($host_get == $tts_ssh[$i][0] )
    {
        $host = $tts_ssh[$i][0];
        $username = $tts_ssh[$i][1];
        $password = $tts_ssh[$i][2];
        break;
    }
}

if(empty($host))
{
    $host = $tts_ssh[0][0];
    $username = $tts_ssh[0][1];
    $password = $tts_ssh[0][2];
}


/// phase 03 : recuperer les commandes


		
require_once('Net/SSH2.php');

$ssh = new Net_SSH2($host);


echo "<p>\n";

echo "host : $host <br />\n";
echo "username : $username <br />\n";
echo "password : $password <br />\n";


echo "tts_bash : <br />\n";


if (!$ssh->login($username, $password))
{
    exit('Login Failed');
}

// die;
		
$cmd_get_bash = 'cat ~/.GoldOrHack/bash.csv.txt';
$csv_bash = $ssh->exec($cmd_get_bash);

// sudo python main_motors.py to_back
// sudo nano bash.csv.txt


/// phase xx : recuperer l'option choisis

$cmd = "";
$option_get = "";

if (isset($_GET["option"]))
{
    $option_get = $_GET["option"];
}


/// phase 03 : recuperer les commandes
$cmd_get_bash = 'cat ~/.GoldOrHack/bash.csv.txt';
$csv_bash = $ssh->exec($cmd_get_bash);
$tts_bash = getTtsFromCsvString($csv_bash);

?><!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
        "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <link rel="shortcut icon" href="../favicon.ico"/>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/>
    <title>menu.php</title>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.0/jquery.min.js"></script>
    <link href="http://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet"/>
    <link href="http://azuril.com/css/materialize/css/materialize.css" rel="stylesheet"/>
    <script src="http://azuril.com/css/materialize/js/materialize.js" type="text/javascript"></script>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <style>
        .btn
        {
            margin: 5px;
        }
    </style>
</head>
<body>
<p><a href="../">retour</a></p>
<h1>raspberrypiKinect</h1>
<hr/>
<h2>menu.php</h2>
<p>

    <?php
	
	$i = 0;
	
	$iMax = count($tts_bash);
	
echo "count(tts_bash) : ".count($tts_bash)." <br />\n";
echo $iMax;
echo " <br />\n";
	
    while ( $i < count($tts_bash))
    {
        $option_menu = $tts_bash[$i][0];
		
		
echo "i : $i <br />\n";
		
echo "option_menu : $option_menu <br />\n";
		
        ?>
        <a class="waves-effect waves-light btn"
           href="?option=<?php echo $option_menu; ?>"><?php echo $option_menu; ?></a><br/>
        <?php
		
        if ($option_get == $option_menu)
        {
            $cmd = $tts_bash[$i][1];
            echo " option a executer : " . $option_get . "<br />";
        }
		
		$i++;
    }
	
echo "i : $i <br />\n";
		
	

    // echo " \$liste_titre_bash : <br /> $csv_bash <br />";
    ?>
</p>
<hr/>
<p>
    <?php

	$isEmpty = empty($option_get);
	
    if ($isEmpty)
	{
		
	}
	else
    {
        $cmd = str_replace("\r", '', $cmd);
        $cmd = str_replace("\n", '', $cmd);
        $command = $cmd;

        try
        {
            require_once('function_ssh.php');
            $output = $ssh->exec($cmd);

        } catch (Exception $e)
        {
            echo 'Exception reçue : ', $e->getMessage(), "\n";
        }

        echo " bash executé : " . $cmd . "<br />";
        echo " output : " . $output . "<br />";
    }

    $ssh->disconnect();

    ?>
</p>
<hr/>


<?php

echo "<h5>debut debug</h5>";

echo "<p>\n";

echo "host : $host <br />\n";
echo "username : $username <br />\n";
echo "cmd : $cmd <br />\n";
echo "option_menu : $option_menu <br />\n";

echo "tts_bash : <br />\n";

print_r($tts_bash);

echo "</p>";

echo "<h5>fin debug</h5>";

?>

<hr/>
<hr/>
</body>
</html>