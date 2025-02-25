# RunGroops

RunGroops is an online platform for runners. This platform helps you find clubs, schedule events, and meet other runners in your area.

## 🏃 Getting Started

### Database Setup
Watch the [YouTube video]([#](https://www.youtube.com/watch?v=af_tK9LUiX0)) on how to set up the database.

### Clone the Repository
Navigate to the directory where you want to keep the project and run the following command:
```sh
 git fork https://github.com/ahmedsaloma/Pokemon.git
```

### Create a Local Database
If you are unsure how to do this, refer to the YouTube video.

### Configure the Connection String
Add the following connection string to `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=DESKTOP-EI2TOGP\\SQLEXPRESS;Initial Catalog=RunGroops;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
  }
}
```

### Set Up Cloudinary
Register for a free Cloudinary account and add the following details to `appsettings.json`:
```json
{
  "CloudinarySettings": {
    "CloudName": "your-cloud-name",
    "ApiKey": "your-api-key",
    "ApiSecret": "your-api-secret"
  }
}
```


Happy Running! 🏃‍♂️🏃‍♀️

