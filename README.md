# 酷意-AI 智慧檔案資料整理 (CoolE-AI Smart File Organizer)

![Project Status](https://img.shields.io/badge/Status-Stable-brightgreen)
![Language](https://img.shields.io/badge/Language-C%23-blue)
![Framework](https://img.shields.io/badge/Framework-WinForms-purple)
![AI](https://img.shields.io/badge/AI-Google%20Gemini-orange)
![License](https://img.shields.io/badge/License-MIT-green)

**酷意-AI 智慧檔案資料整理** 是一款由 **[酷意 AI 平台](http://ourcoolidea.com)** 開發的智慧型桌面整理工具。

本專案使用 C# (Windows Forms) 結合 Google Gemini AI 的語意理解能力，解決使用者桌面充滿雜亂檔案的問題，實現一鍵拖曳、自動分類歸檔。

> **核心理念**：結合自動化與 AI 語意理解，大幅節省整理檔案的時間。

## ✨ 核心功能 (Features)

* **🤖 AI 智慧語意分類**：
    透過整合 `Gemini-2.5-flash` 模型，不只依據副檔名，更根據檔案名稱的語意進行精確分類（例如：將 `.sql` 與專案相關的 `.zip` 統一歸類為「專案開發」）。
* **🚀 極速批次處理**：
    優化的 API 呼叫策略。提取所有檔名後，僅發送一次 API 請求即可獲得所有分類建議，速度提升顯著。
* **🖱️ 直覺式拖曳 (Drag & Drop)**：
    支援直接從 Windows 檔案總管將多個檔案拖入視窗，立即觸發分析。
* **↩️ 完整的還原機制 (Undo)**：
    內建 Stack 結構紀錄移動路徑，提供「還原上一步」功能，操作失誤可隨時恢復。

## 📸 應用展示 (Demo)

### 1. 簡潔的主介面
系統就緒，等待任務。
![主介面](./img/1.png)

### 2. 拖曳檔案進行分析
支援一次拖入大量混雜的檔案（圖片、文件、執行檔）。
![拖曳檔案](./img/2.png)

### 3. AI 批次運算中
即時顯示處理進度與 AI 分析狀態。
![AI分析中](./img/3.png)

### 4. 自動歸檔完成
自動建立分類資料夾（如：應用程式、多媒體、專案開發）並完成移動。
![完成歸檔](./img/4.png)


## 🛠️ 技術架構 (Technical Architecture)

本專案採用 **三層式架構 (3-Tier Architecture)** 設計，以確保程式碼的清晰度與維護性：

### 1. 介面層 (UI Layer - `MainForm`)
* 處理 Windows Forms 的 `DragEnter` 與 `DragDrop` 事件。
* 使用非同步機制 (`Invoke` / `BackgroundWorker`) 更新 UI，避免介面在分析時凍結。

### 2. AI 服務層 (AI Service Layer - `GeminiService`)
* **最佳化策略**：避免對每個檔案單獨呼叫 API。
* **運作邏輯**：
    1.  提取所有待處理檔案的 `FileName`。
    2.  將檔名清單封裝為 JSON Prompt。
    3.  **單次 Request** 發送至 Google Gemini API。
    4.  接收並解析回傳的分類 JSON (包含 `Category` 與 `FileName`)。

### 3. 檔案操作層 (File Handler Layer)
* **安全移動**：執行 `File.Move` 前自動檢查目標路徑。
* **衝突處理**：若目標資料夾已有同名檔案，自動重新命名 (e.g., `file(1).txt`)。
* **還原管理**：使用 `Stack<MoveRecord>` 記錄來源與目的路徑，實現 LIFO (後進先出) 的還原邏輯。

## 🚀 快速開始 (Getting Started)

### 前置需求
* Visual Studio 2019 或更高版本
* .NET Framework 4.7.2+
* 有效 Google Cloud Project 帳號並啟用 **Gemini API**

### 安裝與執行
1.  **Clone 專案**
    ```bash
    git clone [https://github.com/your-username/CoolE-AI-File-Organizer.git](https://github.com/your-username/CoolE-AI-File-Organizer.git)
    ```
2.  **設定 API Key**
    在程式設定頁面或 `app.config` 中填入您的 Google Gemini API Key。
3.  **編譯執行**
    使用 Visual Studio 開啟 `.sln` 檔並執行。

## 📝 授權 (License)

本專案採用 **MIT License** 授權。詳細內容請參閱 `LICENSE` 文件。

---

<p align="center">
  <strong>Created by Joseph</strong><br>
  🌐 <a href="http://ourcoolidea.com">酷意 AI 平台 (ourcoolidea.com)</a>
</p>
