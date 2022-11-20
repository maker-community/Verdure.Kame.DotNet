# Verdure.Kame.DotNet
基于DotNet和树莓派Kame四足机器人。



本项目是复刻国外的一个[RaspberryPi-Minikame](https://github.com/LakshBhambhani/RaspberryPi-Minikame)的四足项目。

由于原项目是python版本的代码，我就想试试dotnet iot怎么样，就照着进行复刻了，然后用dotnet实现服务代码和控制代码，翻遍社区没找到dotnet实现的我购买的172*320尺寸的屏幕驱动，就采用用dotnet iot库进行实现了，屏幕驱动地址[dotnet-iot-tutorial-code](https://github.com/GreenShadeZhang/dotnet-iot-tutorial-code)。

软件架构图如下：

![architecture-diagram](/Images/architecture-diagram.png)

部署在树莓派上的服务通过驱动和16路舵机，显示屏进行通讯。

电脑端通过MAUI编写的APP解析图片和视频数据，再结合控制指令发送到Grpc服务。

推荐项目

[RaspberryPi-Minikame](https://github.com/LakshBhambhani/RaspberryPi-Minikame)

[dotnet-iot-tutorial-code](https://github.com/GreenShadeZhang/dotnet-iot-tutorial-code)


[MiniKame](https://github.com/JavierIH/miniKame)
