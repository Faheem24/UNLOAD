<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Config2.aspx.cs" Inherits="UNLOAD_Web_UI.Config2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <meta http-equiv="x-ua-compatible" content="ie=edge">

  <title>UNLOAD | Config</title>

  <!-- Font Awesome Icons -->
  <link rel="stylesheet" href="plugins/fontawesome-free/css/all.min.css">
  <!-- Theme style -->
  <link rel="stylesheet" href="dist/css/adminlte.min.css">
  <!-- Google Font: Source Sans Pro -->
  <link href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700" rel="stylesheet">
</head>
<body class="hold-transition sidebar-mini">
    <form id="form1" runat="server">
        <div class="wrapper">
              <!-- Navbar -->
  <nav class="main-header navbar navbar-expand navbar-white navbar-light">
    <!-- Left navbar links -->
    <ul class="navbar-nav">
      <li class="nav-item">
        <a class="nav-link" data-widget="pushmenu" href="#" role="button"><i class="fas fa-bars"></i></a>
      </li>
      <li class="nav-item d-none d-sm-inline-block">
        <a href="Home2.aspx" class="nav-link">Home</a>
      </li>
      <li class="nav-item d-none d-sm-inline-block">
        <a href="Config2.aspx" class="nav-link">Settings</a>
      </li>
    </ul>

    <!-- SEARCH FORM -->
    <!--
      <form class="form-inline ml-3">
      <div class="input-group input-group-sm">
        <input class="form-control form-control-navbar" type="search" placeholder="Search" aria-label="Search">
        <div class="input-group-append">
          <button class="btn btn-navbar" type="submit">
            <i class="fas fa-search"></i>
          </button>
        </div>
      </div>
    </form>
      -->

    <!-- Right navbar links -->    
      <ul class="navbar-nav ml-auto">
      <!-- Messages Dropdown Menu -->
      <li class="nav-item dropdown">
        <a class="nav-link" data-toggle="dropdown" href="#">
          <i class="far fa-comments"></i>
          <span class="badge badge-danger navbar-badge">3</span>
        </a>
        <div class="dropdown-menu dropdown-menu-lg dropdown-menu-right">
          <a href="Home2.aspx" class="dropdown-item">
            
      <!-- Notifications Dropdown Menu -->
      <li class="nav-item dropdown">
        <a class="nav-link" data-toggle="dropdown" href="#">
          <i class="far fa-bell"></i>
          <span class="badge badge-warning navbar-badge">15</span>
        </a>
        <div class="dropdown-menu dropdown-menu-lg dropdown-menu-right">
          <span class="dropdown-header">15 Notifications</span>
          <div class="dropdown-divider"></div>
          <a href="#" class="dropdown-item">
            <i class="fas fa-envelope mr-2"></i> 4 new messages
            <span class="float-right text-muted text-sm">3 mins</span>
          </a>
          <div class="dropdown-divider"></div>
          <a href="#" class="dropdown-item">
            <i class="fas fa-users mr-2"></i> 8 friend requests
            <span class="float-right text-muted text-sm">12 hours</span>
          </a>
          <div class="dropdown-divider"></div>
          <a href="#" class="dropdown-item">
            <i class="fas fa-file mr-2"></i> 3 new reports
            <span class="float-right text-muted text-sm">2 days</span>
          </a>
          <div class="dropdown-divider"></div>
          <a href="#" class="dropdown-item dropdown-footer">See All Notifications</a>
        </div>
      </li>
      <li class="nav-item">
        <a class="nav-link" data-widget="control-sidebar" data-slide="true" href="#" role="button"><i
            class="fas fa-th-large"></i></a>
      </li>
    </ul>
  </nav>
  <!-- /.navbar -->

  <!-- Main Sidebar Container -->
  <aside class="main-sidebar sidebar-dark-primary elevation-4">
    <!-- Brand Logo -->
    <a href="Home2.aspx" class="brand-link">
      <img src="dist/img/ULogo.jpg" alt="AdminLTE Logo" class="brand-image img-circle elevation-3"
           style="opacity: .8">
      <span class="brand-text font-weight-light">UNLOAD</span>
    </a>

    <!-- Sidebar -->
    <div class="sidebar">
      <!-- Sidebar user panel (optional) -->
      <div class="user-panel mt-3 pb-3 mb-3 d-flex">
        <div class="image">
          <img src="dist/img/ULogo.jgp" class="img-circle elevation-2" alt="User Image">
        </div>
        <div class="info">
          <a href="#" class="d-block">Sana Sajjad</a>
        </div>
      </div>

      <!-- Sidebar Menu -->
      <nav class="mt-2">
        <ul class="nav nav-pills nav-sidebar flex-column" data-widget="treeview" role="menu" data-accordion="false">
          <!-- Add icons to the links using the .nav-icon class
               with font-awesome or any other icon font library -->
          <li class="nav-item has-treeview menu-open">
            <a href="#" class="nav-link active">
              <i class="nav-icon fas fa-tachometer-alt"></i>
              <p>
                Analysis
                <i class="right fas fa-angle-left"></i>
              </p>
            </a>
            <ul class="nav nav-treeview">
              <li class="nav-item">
                <a href="Log2.aspx" class="nav-link">
                  <i class="far fa-circle nav-icon"></i>
                  <p>Logs</p>
                </a>
              </li>
              <li class="nav-item">
                <a href="Alert2.aspx" class="nav-link">
                  <i class="far fa-circle nav-icon"></i>
                  <p>Alerts</p>
                </a>
              </li>
            </ul>
          </li>
          <li class="nav-item">
            <a href="Config2.aspx" class="nav-link">
              <i class="nav-icon fas fa-th"></i>
              <p>
                Settings
                <!--
                  <span class="right badge badge-danger">New</span>
                  -->
              </p>
            </a>
          </li>
        </ul>
      </nav>
      <!-- /.sidebar-menu -->
    </div>
    <!-- /.sidebar -->
  </aside>

  <!-- Content Wrapper. Contains page content -->
  <div class="content-wrapper">
    <!-- Content Header (Page header) -->
    <div class="content-header">
      <div class="container-fluid">
        <div class="row mb-2">
          <div class="col-sm-6">
            <h1 class="m-0 text-dark">UNLOAD Configuration Settings</h1>
          </div><!-- /.col -->
          <div class="col-sm-6">
            <ol class="breadcrumb float-sm-right">
              <li class="breadcrumb-item"><a href="Home.aspx">Home</a></li>
            </ol>
          </div><!-- /.col -->
        </div><!-- /.row -->
      </div><!-- /.container-fluid -->
    </div>
    <!-- /.content-header -->

    <!-- Main content -->
    <div class="content">
      <div class="container-fluid">
        <div class="row">
          <div class="col-lg-6">
            <div class="card card-primary card-outline">
              <div class="card-body">
                <h5 class="card-title">1. Password Guess</h5>
                <br />
                <asp:Label ID="Label1" runat="server" Text="User Name"></asp:Label>
                <asp:TextBox ID="UserName1" runat="server"></asp:TextBox>
                <br />
                <asp:Label ID="Label2" runat="server" Text="No. of Failed Attempts in 1 Min."></asp:Label>
                <asp:TextBox ID="Attempts1" runat="server"></asp:TextBox>
                <br />
                <asp:Button ID="Update1" runat="server" Text="Update" OnClick="Update1_Click" />
                <asp:Button ID="Clear1" runat="server" Text="Clear" OnClick="Clear1_Click" />
              </div>
            </div>

              <div class="card card-primary card-outline">
              <div class="card-body">
                <h5 class="card-title">2. Non Office Hours Login</h5>
                <br />
                <asp:Label ID="Label3" runat="server" Text="User Name"></asp:Label>
                <asp:TextBox ID="UserName2" runat="server"></asp:TextBox>
                <br />
                <asp:Label ID="Label4" runat="server" Text="Start Time"></asp:Label>
                <asp:TextBox ID="Start2" runat="server"></asp:TextBox>
                <br />
                <asp:Label ID="Label5" runat="server" Text="End Time"></asp:Label>
                <asp:TextBox ID="End2" runat="server"></asp:TextBox>
                <br />
                <asp:Button ID="Update2" runat="server" Text="Update" OnClick="Update2_Click" />
                <asp:Button ID="Clear2" runat="server" Text="Clear" OnClick="Clear2_Click" />
              </div>
            </div>

            <div class="card card-primary card-outline">
              <div class="card-body">
                <h5 class="card-title">3. Outside Remote Login</h5>
                <br />
                <asp:Label ID="Label6" runat="server" Text="Asset IP"></asp:Label>
                <asp:TextBox ID="Asset3" runat="server"></asp:TextBox>
                <br />
                <asp:Label ID="Label7" runat="server" Text="Start IP"></asp:Label>
                <asp:TextBox ID="Start3" runat="server"></asp:TextBox>
                <br />
                <asp:Label ID="Label8" runat="server" Text="End IP"></asp:Label>
                <asp:TextBox ID="End3" runat="server"></asp:TextBox>
                <br />
                <asp:Button ID="Update3" runat="server" Text="Update" OnClick="Update3_Click" />
                <asp:Button ID="Clear3" runat="server" Text="Clear" OnClick="Clear3_Click" />
              </div>
            </div><!-- /.card -->
          </div>
          <!-- /.col-md-6 -->
          
          <!-- /.col-md-6 -->
        </div>
        <!-- /.row -->
      </div><!-- /.container-fluid -->
    </div>
    <!-- /.content -->
  </div>
  <!-- /.content-wrapper -->

  <!-- Control Sidebar -->
  <aside class="control-sidebar control-sidebar-dark">
    <!-- Control sidebar content goes here -->
    <div class="p-3">
      <h5>Title</h5>
      <p>Sidebar content</p>
    </div>
  </aside>
  <!-- /.control-sidebar -->

  <!-- Main Footer -->
  <footer class="main-footer">
    <!-- To the right -->
    <div class="float-right d-none d-sm-inline">
      UNauthorized LOgin Attack Detector
    </div>
    <!-- Default to the left -->
    <strong>Copyright &copy; 2020 <a>UNLOAD</a>.</strong> All rights reserved.
  </footer>
        </div>
        <!-- ./wrapper -->

<!-- REQUIRED SCRIPTS -->

<!-- jQuery -->
<script src="plugins/jquery/jquery.min.js"></script>
<!-- Bootstrap 4 -->
<script src="plugins/bootstrap/js/bootstrap.bundle.min.js"></script>
<!-- AdminLTE App -->
<script src="dist/js/adminlte.min.js"></script>
    </form>
</body>
</html>
