<?php

$cmd = 'sudo /usr/bin/python2.7 /home/pi/Desktop/AZURIL/hat/hatSay.py vide 2>&1     ';

$option_get = "vide";

if (isset($_GET["option"]))
{
    $option_get = $_GET["option"];
}

$csvPath = "../config/bash.csv.txt";

// Lit une page web dans un tableau.
$lines = file($csvPath);

/// array str_getcsv ( string $input [, string $delimiter = "," [, string $enclosure = '"' [, string $escape = "\\" ]]] )
// $ts_csv = str_getcsv($csvPath, ";");


// print_r($lines);


// $cmd = 'sudo /usr/bin/python2.7 /home/pi/Desktop/AZURIL/hat/hatSay.py '.$msg.' 2>&1';



?><!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
        "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <link rel="shortcut icon" href="../favicon.ico" />

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

<p>
    <a href="../" >retour</a>
</p>

<h1>
    raspberrypiKinect
</h1>

<h2>
    menu.php
</h2>

<p>

    maj le 2016-12-15 a 22h41 <br/>


    <?php

    for ($i = 0; $i < count($lines); $i++)
    {

        $option_menu = explode(";", $lines[$i])[0];

        ?>

        <a class="waves-effect waves-light btn"
           href="menu.php?option=<?php echo $option_menu; ?>"><?php echo $option_menu; ?></a><br/>

        <?php

        if ($option_get == $option_menu)
        {
            $cmd = explode(";",
                $lines[$i])[1];

            echo " option a executer : " . $option_get . "<br />";
        }

    }

    ?>

<hr/>

<?php

// shell_exec('sudo -u root -S '.$cmd.' < /home/pi/sudopass.secret');


ob_start();
passthru($cmd);
$output = ob_get_clean();


echo " bash executé : " . $cmd . "<br />";
echo " output : " . $output . "<br />";

?>

<hr/>
<hr/>


</p>

</body>
</html>

