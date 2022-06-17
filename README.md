# ActivPass
ActivPass is a simple and local password management tool

## Prerequisites / Tools

The following software is required to run or compile ActivPass.

* Windows OS
* .NET Framework 4.6.1
* Visual Studio

## Functionality

### Usage
Password items are grouped into so-called "Containers". For safety reasons,
a container is an isolated unit. This means a password item exists in only
one specific container. Items can not be shared between containers. ActivPass
stores containers as an encrypted file on the local disk. The so-called
"master password" is used to open a container (decrypt the container).
The master password is set initially when the container is created. When you
lose the master password it is impossible to open the container again. It is
important to use a strong password as the master password, to secure the container
against brute force attacks.

### Storage
Each container is stored as a binary file (*.bin) in `AppData\Roaming\ActivPass`.
Container files are encrypted with AES-256 CBC. The key for the encryption is the
first 32 bytes block of the SHA256 hashed master password. When the container is
unlocked, the container data are stored in memory. Data on disk is always encrypted.
You can backup and restore containers, simply by copying and replacing the container
files. It is possible to export unlocked containers as unencrypted CSV documents, for
backup or exchange purposes.

## Configuration
The application stores the configuration in the file `Configuration.xml` in
`AppData\Roaming\ActivPass`. The configuration can be changed in the user interface.
When no configuration exists, a default configuration is used and a new configuration
file will be created.