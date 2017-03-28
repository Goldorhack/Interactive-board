<?php

require_once('function_ssh.php');


/// File : ssh02.php <br/>
/// Author : Felix ROBICHON <br/>
/// Create Date : 2017-02-06 <br/>
/// Last Update : 2017-02-06 <br/>

/// phase 01 : recuperer le titre de la commande si elle existe



$cmd = "";
$option_get = "";

if (isset($_GET["option"]))
{
    $option_get = $_GET["option"];
}

/// phase 02 : recuperer les données de connection
require_once('../config/ssh_serv_list.php');
$tts_ssh = getTtsFromCsvString($ssh_csv_info);
$host = $tts_ssh[0][0];
$username = $tts_ssh[0][1];
$password = $tts_ssh[0][2];

/// phase 03 : recuperer les commandes
$cmd_get_bash = 'cat ~/.GoldOrHack/bash.csv.txt';
$csv_bash = ssh_exec($host, $username, $password, $cmd_get_bash);
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
    for ($i = 0; $i < count($tts_bash); $i++)
    {
        $option_menu = $tts_bash[$i][0];
        ?>
        <a class="waves-effect waves-light btn"
           href="?option=<?php echo $option_menu; ?>"><?php echo $option_menu; ?></a><br/>
        <?php
        if ($option_get == $option_menu)
        {
            $cmd = $tts_bash[$i][1];
            echo " option a executer : " . $option_get . "<br />";
        }
    }

    // echo " \$liste_titre_bash : <br /> $csv_bash <br />";
    ?>
</p>
<hr/>
<p>
    <?php

    if (!empty($option_get))
    {
        $cmd = str_replace("\r", '', $cmd);
        $cmd = str_replace("\n", '', $cmd);
        $command = $cmd;

        try
        {
            require_once('function_ssh.php');
            $output = ssh_exec($host, $username, $password, $cmd);

        } catch (Exception $e)
        {
            echo 'Exception reçue : ', $e->getMessage(), "\n";
        }

        echo " bash executé : " . $cmd . "<br />";
        echo " output : " . $output . "<br />";
    }
    ?>
</p>
<hr/>
<hr/>
</body>
</html>