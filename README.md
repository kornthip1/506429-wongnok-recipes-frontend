# 506429-wongnok-recipes-frontend
frontend Devpool
สำหรับ frontend รันบน local รันผ่าน visual studio version 2019 ขึ้นไป
ติดตั้ง .net sdk  .net framwork 4.6.1
เปิดด้วยไฟล์ .sln
เมื่อลงแล้วและเปิด project แล้ว ติดตั้ง Pakget  ด้วย Nuget Pakget  สำหรับ ติดตั้ง Newtonsoft.Json


แล้ว เปลี่ยน connection endoint ท ไฟล์ที่ชื่อว่า Controllers\HomeController.cs 
parameter ชื่อว่า endpoint

เมื่อเร้อยแล้ว กดรัน จะแสดงหน้าเว็บแอพพลิเคชั่น


การใช้งาน
1. เข้าหน้าแรกแสดงรายการทั้งหมด
2. signup เพื่อสมัคร member
3. login ด้วย email ที่สมัครเอาไว้
4. เพิ่มสูตรเมนูที่ ปุ่มชื่อ ผู้ใช้งาน
5. กรอกเมนู รายละเอียดและภาพ
6. ที่เมนู สูตรของฉัน แสดงรายการที่เพิ่มเอาไว้  สามารถ แก้ไข และ ลบรายการได้
7. เมนู หน้าแรก จะไปยังหน้าแรกแสดงรายการสูตรทั้งหมด หากlogin แล้วจะสามารถให้คะแนนสูตรของ member คนอื่นๆได้
