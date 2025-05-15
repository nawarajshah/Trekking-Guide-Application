# TrekkingGuideApp

TrekkingGuideApp is a web application designed to help users discover, manage, and share information about trekking places. The app supports user roles such as Admin, SuperAdmin, and Guide, each with tailored features for managing places and users.

## Features

- Browse a curated list of trekking places with images and descriptions.
- Admin and SuperAdmin users can manage places and user accounts.
- Guides have access to specialized tools for searching and managing trekking locations.
- User authentication and role-based navigation.
- Rich text editing for place descriptions using TinyMCE.
- Image upload and preview for trekking places.

## Technologies Used

- ASP.NET Core MVC (C#)
- Razor Views
- Bootstrap for responsive UI
- TinyMCE for WYSIWYG editing
- Hashids for obfuscating place IDs
- Angular (in `wwwroot/Angular/TrekApp` for SPA features)

## Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)
- [Node.js and npm](https://nodejs.org/) (for Angular frontend)
- Visual Studio 2022

### Running the ASP.NET Core App

1. Open the solution in Visual Studio 2022.
2. Restore NuGet packages.
3. Build and run the project (`F5` or `Ctrl+F5`).
4. Navigate to `https://localhost:<port>/` in your browser.

### Running the Angular App

1. Navigate to `wwwroot/Angular/TrekApp` in your terminal.
2. Run `npm install` to install dependencies.
3. Start the development server with `ng serve`.
4. Open `http://localhost:4200/` in your browser.

## Usage

- Register or log in to access features.
- Admins can add, edit, or delete trekking places and manage users.
- Guides can search for places and use guide-specific tools.
- All users can view place details and upload images.

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss your ideas.

## License

This project is licensed under the MIT License.

