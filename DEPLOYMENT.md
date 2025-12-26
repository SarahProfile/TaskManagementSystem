# Deployment Guide

## Frontend Deployment (Vercel)

### Prerequisites
- GitHub account
- Vercel account (sign up with GitHub)

### Steps

1. **Push Code to GitHub** ✅ (Already completed)

2. **Deploy to Vercel**
   - Go to https://vercel.com
   - Sign in with GitHub
   - Click "Add New Project"
   - Select `TaskManagementSystem` repository
   - Configure:
     - **Root Directory**: `frontend`
     - **Framework**: Next.js (auto-detected)
     - **Build Command**: `npm run build`
     - **Output Directory**: `.next`
   - Add Environment Variable:
     - `NEXT_PUBLIC_API_URL`: Your backend API URL (e.g., `https://api.yourdomain.com/api`)
   - Click "Deploy"

3. **Update Environment Variable**
   - After backend deployment, update `NEXT_PUBLIC_API_URL` in Vercel
   - Go to Project Settings → Environment Variables
   - Edit `NEXT_PUBLIC_API_URL` to point to your deployed backend
   - Redeploy the application

---

## Backend Deployment Options

### Option 1: Hostinger VPS (Recommended if you have VPS)

**Requirements:**
- Hostinger VPS plan
- SSH access
- Root or sudo privileges

**Deployment Steps:**

#### 1. Connect to Your VPS
```bash
ssh root@your-vps-ip
```

#### 2. Install .NET 8 Runtime
```bash
# Add Microsoft package repository
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb

# Install .NET 8 Runtime
sudo apt-get update
sudo apt-get install -y aspnetcore-runtime-8.0

# Verify installation
dotnet --version
```

#### 3. Publish Your Application Locally
```bash
# On your local machine, navigate to the API project
cd /Users/sarah/TaskManagementSystem/backend/src/TaskManagement.API

# Publish for Linux
dotnet publish -c Release -o ./publish --runtime linux-x64 --self-contained false
```

#### 4. Upload to VPS
```bash
# Create application directory on VPS
ssh root@your-vps-ip "mkdir -p /var/www/taskmanagement"

# Upload published files (run from local machine)
scp -r ./publish/* root@your-vps-ip:/var/www/taskmanagement/
```

#### 5. Configure Production Settings
```bash
# Edit appsettings.Production.json on VPS
ssh root@your-vps-ip
cd /var/www/taskmanagement
nano appsettings.Production.json
```

Update:
- `JwtSettings.SecretKey`: Use a strong, unique secret key
- `CorsSettings.AllowedOrigins`: Add your Vercel frontend URL

#### 6. Create Systemd Service
```bash
sudo nano /etc/systemd/system/taskmanagement.service
```

Add this content:
```ini
[Unit]
Description=Task Management API
After=network.target

[Service]
Type=notify
WorkingDirectory=/var/www/taskmanagement
ExecStart=/usr/bin/dotnet /var/www/taskmanagement/TaskManagement.API.dll
Restart=always
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=taskmanagement-api
User=www-data
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=ASPNETCORE_URLS=http://0.0.0.0:5000

[Install]
WantedBy=multi-user.target
```

#### 7. Start the Service
```bash
# Reload systemd
sudo systemctl daemon-reload

# Enable service to start on boot
sudo systemctl enable taskmanagement.service

# Start the service
sudo systemctl start taskmanagement.service

# Check status
sudo systemctl status taskmanagement.service

# View logs
sudo journalctl -u taskmanagement.service -f
```

#### 8. Configure Nginx Reverse Proxy
```bash
sudo apt-get install nginx
sudo nano /etc/nginx/sites-available/taskmanagement
```

Add this configuration:
```nginx
server {
    listen 80;
    server_name api.yourdomain.com;

    location / {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }
}
```

Enable the site:
```bash
sudo ln -s /etc/nginx/sites-available/taskmanagement /etc/nginx/sites-enabled/
sudo nginx -t
sudo systemctl restart nginx
```

#### 9. Install SSL Certificate (Optional but Recommended)
```bash
sudo apt-get install certbot python3-certbot-nginx
sudo certbot --nginx -d api.yourdomain.com
```

#### 10. Configure Firewall
```bash
sudo ufw allow 'Nginx Full'
sudo ufw allow OpenSSH
sudo ufw enable
```

---

### Option 2: Alternative Cloud Providers (If Hostinger is Shared Hosting)

If you have Hostinger **shared hosting** (not VPS), .NET applications won't work. Consider these alternatives:

#### A. Azure App Service (Recommended for .NET)
**Pros:**
- Native .NET support
- Easy deployment
- Free tier available
- Microsoft ecosystem

**Steps:**
1. Create Azure account (free tier)
2. Create App Service (Web App)
3. Deploy from GitHub or VS Code
4. Configure connection strings and app settings

**Cost:** Free tier available, paid plans start at ~$13/month

#### B. Railway.app
**Pros:**
- Simple deployment from GitHub
- Supports .NET
- Free tier: $5 credit/month
- Automatic HTTPS

**Steps:**
1. Sign up at https://railway.app
2. "New Project" → "Deploy from GitHub"
3. Select your repository
4. Add `Dockerfile` to backend (Railway will auto-detect)
5. Add environment variables

**Cost:** Free $5/month credit, pay-as-you-go after

#### C. Render.com
**Pros:**
- Free tier available
- Deploy from GitHub
- Automatic HTTPS
- PostgreSQL database included

**Steps:**
1. Sign up at https://render.com
2. "New Web Service"
3. Connect GitHub repository
4. Configure build settings
5. Add environment variables

**Cost:** Free tier available

#### D. DigitalOcean App Platform
**Pros:**
- Easy deployment
- Managed service
- Good documentation
- Scalable

**Cost:** Starts at $5/month

---

## Post-Deployment Configuration

### 1. Update Frontend Environment Variable
In Vercel dashboard:
- Go to your project
- Settings → Environment Variables
- Update `NEXT_PUBLIC_API_URL` to your backend URL
- Redeploy the frontend

### 2. Update Backend CORS
In `appsettings.Production.json`:
```json
"CorsSettings": {
  "AllowedOrigins": [
    "https://your-vercel-app.vercel.app"
  ]
}
```

### 3. Generate Strong JWT Secret
```bash
# Generate a secure random key
openssl rand -base64 32
```
Update in `appsettings.Production.json`

### 4. Test the Deployment
- Visit your Vercel frontend URL
- Try to register a new account
- Login with test credentials
- Create tasks and projects
- Check browser console for errors

---

## Environment Variables Summary

### Frontend (Vercel)
```
NEXT_PUBLIC_API_URL=https://api.yourdomain.com/api
```

### Backend (Production)
In `appsettings.Production.json`:
- `ConnectionStrings.DefaultConnection`: Path to SQLite database
- `JwtSettings.SecretKey`: Strong random secret (32+ characters)
- `CorsSettings.AllowedOrigins`: Array with your Vercel frontend URL

---

## Troubleshooting

### Frontend can't connect to backend
- Check CORS settings in backend
- Verify `NEXT_PUBLIC_API_URL` in Vercel
- Check browser console for errors
- Ensure backend is running and accessible

### Backend not starting
- Check logs: `sudo journalctl -u taskmanagement.service -f`
- Verify .NET runtime is installed: `dotnet --version`
- Check file permissions: `sudo chown -R www-data:www-data /var/www/taskmanagement`
- Verify database path in appsettings.Production.json

### 500 Internal Server Error
- Check backend logs
- Verify database exists and is accessible
- Check appsettings.Production.json configuration
- Ensure all dependencies are published

### CORS errors
- Add your frontend URL to `CorsSettings.AllowedOrigins`
- Restart the backend service
- Clear browser cache
- Check that URL includes protocol (https://)

---

## Security Checklist

- [ ] Change JWT SecretKey to a strong random value
- [ ] Enable HTTPS (SSL certificate)
- [ ] Configure firewall rules
- [ ] Set proper file permissions
- [ ] Use environment variables for secrets
- [ ] Enable rate limiting (consider adding middleware)
- [ ] Regular security updates
- [ ] Database backups
- [ ] Monitor logs for suspicious activity

---

## Maintenance

### Update Application
```bash
# On local machine: publish new version
dotnet publish -c Release -o ./publish --runtime linux-x64 --self-contained false

# Upload to VPS
scp -r ./publish/* root@your-vps-ip:/var/www/taskmanagement/

# On VPS: restart service
sudo systemctl restart taskmanagement.service
```

### Database Backup
```bash
# Backup SQLite database
cp /var/www/taskmanagement/TaskManagement.db /var/www/taskmanagement/backups/TaskManagement-$(date +%Y%m%d).db
```

### View Logs
```bash
# Application logs
sudo journalctl -u taskmanagement.service -f

# Nginx logs
sudo tail -f /var/log/nginx/access.log
sudo tail -f /var/log/nginx/error.log
```
