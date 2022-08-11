<%@ Page Language="C#" AutoEventWireup="true" %>
<%@ Import Namespace="System.Security.Cryptography" %>
<%@ Import Namespace="System.Threading" %>


<script runat="server">

// http://www.microsoft.com/technet/security/advisory/2416728.mspx

        void Page_Load() {
        byte[] delay = new byte[1];
        RandomNumberGenerator prng = new RNGCryptoServiceProvider();

        prng.GetBytes(delay);
        Thread.Sleep((int)delay[0]);
        
        IDisposable disposable = prng as IDisposable;
        if (disposable != null) { disposable.Dispose(); }
    }
	
</script>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Oops!</title>
    <style>
        body{font-family:Tahoma,Arial,sans-serif;font-size:15px;height:100%;background-color:#f1f9fb}body .error-page{text-align:center;margin-top:50px}body .error-page .name{font-size:25px}.error-page .error-content p{line-height:25px}.error-page .error-content a{white-space:nowrap;color:#00f;text-decoration:none}.error-page .error-content a:hover{text-decoration:underline}
    </style>
</head>
<body class="sidebar-mini">
    <div class="error-page">
        <div class="error-content">
            <h3 class="name">
                <i class="fas fa-exclamation-triangle text-danger"></i> Có lỗi xảy ra trong quá trình xử lý.
            </h3>
            <p>
                Lỗi đã được ghi nhận. Trong lúc chúng tôi giải quyết sự cố này, bạn có thể Trở lại <a href="/">TRANG CHỦ</a> để xem vài thông tin mới
            </p>
        </div>
    </div>
</body>
</html>