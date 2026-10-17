using UnityEngine;
using System.Collections;
using UnityEditor.PackageManager.UI;
using NUnit.Framework.Constraints;
using UnityEditor.Rendering;
using UnityEngine.UIElements;
using TMPro;

public class ButtonManager : MonoBehaviour
{
    //other classes
   public ScreenFader screenFader;
   public Inventory inventory;
   //front views
   public GameObject frontView;
   public GameObject frontWindowView; 
   public GameObject frontOpenWindowView;     
   public GameObject frontDeskView;
   public GameObject frontComputerView;
   public GameObject frontComputerUnlockView;
   public GameObject frontMedalView;
   public GameObject frontStandView;
   public GameObject frontChestView;
   public GameObject frontDrawerView;
   public GameObject frontCatMoveView;

   //left views
   public GameObject leftView;
   public GameObject leftClosetView;
   public GameObject leftClosetUnlockedView;
   public GameObject leftClosetUnlockednoToyView;
   //right views
   public GameObject rightView;
   public GameObject rightPaintingView;
   public GameObject rightPaintingUnlockedView;
   public GameObject rightBookshelfView;
   public GameObject rightBoardView;
   //down views
   public GameObject downView;
   public GameObject downScrewDriverView;
   public GameObject downScrewlessDriverView;
   public GameObject doorView;
   public GameObject backyardView; //fix this later
   //buttons
   public GameObject leftButton;
   public GameObject downButton;
   public GameObject rightButton;
   public GameObject upButton;
   public GameObject rugButton;
   public GameObject windowButton;
   public GameObject bigWindowButton;
   public GameObject screwdriverButton;
   public GameObject goBackyardButton;
   public GameObject goDeskButton;
   public GameObject goMedalButton;
   public GameObject goChesteButton;
   public GameObject goStandButton;
   public GameObject computerButton;
   public GameObject paintingButton;
   public GameObject bookshelfButton;
   public GameObject submitCompCodeButton;
   public GameObject goBoardButton;
   public GameObject goDrawersButton;
   public GameObject goClosetButton;
   public GameObject goGameButton;
   public GameObject goLetterButton;
   public GameObject goStandPopButton;
   public GameObject goBookPopButton;
   public GameObject collectDrawercoins;
   public GameObject submitCrowCoin;
   public GameObject submitClosetKey;
   public GameObject getToy;
   public GameObject catButton;
   public GameObject goPapersButton;
   public GameObject ChestKeyButton;

   //computer password
   public TMP_InputField computerCodeInput;
   public string computerCode = "1234";
   public string wrongPasswordMessage = "Wrong password";
   public Color wrongPasswordColor = new Color(0.9f, 0.2f, 0.2f);
   public float wrongPasswordFontSize = 48f;
   private TMP_Text passwordErrorText;

   //temp buttons
   public GameObject tempComputerCodeFinishButton;
   public GameObject tempPaintingButton;
   public GameObject submitPaintingButton;
   public GameObject tempBoardButton;
   public GameObject submitBoardButton;
   public GameObject tempGetKeyFromCrow;

   //pop ups
   public GameObject popComputerGame;
   public GameObject popComputerLetter;
   public GameObject popStandSort;
   public GameObject popBookSort;
   public GameObject popPapers;
   //pop up assets
   public GameObject XComputer;
   public GameObject XLetter;
   public GameObject XBook;
   public GameObject Xstand;
   public GameObject xPapers;
   public GameObject TempCompSolved;
   public GameObject TempBookSolved;
   public GameObject TempStandSolved;
   public GameObject TempCompSolvedButton;
   public GameObject TempBookSolvedButton;
   public GameObject TempStandSolvedButton;
   //pop up dialogue
   public GameObject crowPopDialogue;
   public GameObject standDialogue;
   public GameObject catDialogue;
   public GameObject boardDialogue;
   public GameObject medalDialogue;
   public GameObject compDialogue;
   public GameObject capDialogue;
   public GameObject bookDialogue;

   //solve conditions
   private bool screwdriverTaken = false;
   private bool screwdriverUsed = false;
   private bool boardPuzzle = false;
   private bool computerPasswrod = false;
   private bool paintingPuzzle = false;
   private bool submittedGoldCoins = false;
   private bool closetUnlocked = false;
   private bool toyCollected = false;



    /*
    frontView.SetActive(false);
    rightButton.SetActive(false);
    leftButton.SetActive(false);
    goDeskButton.SetActive(false);
    goChesteButton.SetActive(false);
    goStandButton.SetActive(false);
    goMedalButton.SetActive(false); 
    windowButton.SetActive(false);
    */

    void Start()
    {
        if (computerCodeInput == null)
            createComputerCodeInput();

        createPasswordErrorText();
    }

    public void screenFading()
    {
        StartCoroutine(FadeSequence());
    }

    IEnumerator FadeSequence()
    {
        screenFader.FadeOut();
        yield return new WaitForSeconds(0.5f);
        screenFader.FadeIn();
    }  

    public void leftMain()
    {
        if (frontView.activeSelf)
        {
            frontView.SetActive(false);
            catDialogue.SetActive(false);
            leftView.SetActive(true);
            downButton.SetActive(false);
            leftButton.SetActive(false);
            windowButton.SetActive(false);
            goDeskButton.SetActive(false);
            goClosetButton.SetActive(true);
            goMedalButton.SetActive(false);
            goChesteButton.SetActive(false);
            goStandButton.SetActive(false);
            goBoardButton.SetActive(false);
            catButton.SetActive(false);
        }

        if (rightView.activeSelf)
        {
            rightView.SetActive(false);
            frontView.SetActive(true);
            downButton.SetActive(true);
            rightButton.SetActive(true);
            windowButton.SetActive(true);
            goDeskButton.SetActive(true);
            paintingButton.SetActive(false);
            bookshelfButton.SetActive(false);
            goChesteButton.SetActive(true);
            goMedalButton.SetActive(true);
            goStandButton.SetActive(true);
            goBoardButton.SetActive(false);
            catButton.SetActive(true);

        }
    }

    public void rightMain()
    {
        if (frontView.activeSelf)
        {
            frontView.SetActive(false);
            catDialogue.SetActive(false);
            rightView.SetActive(true);
            downButton.SetActive(false);
            rightButton.SetActive(false);
            windowButton.SetActive(false);
            goDeskButton.SetActive(false);
            paintingButton.SetActive(true);
            bookshelfButton.SetActive(true);
            goChesteButton.SetActive(false);
            goMedalButton.SetActive(false);
            goStandButton.SetActive(false);
            goBoardButton.SetActive(true);
            catButton.SetActive(false);
        }

        if (leftView.activeSelf)
        {
            leftView.SetActive(false);
            frontView.SetActive(true);
            downButton.SetActive(true);
            leftButton.SetActive(true);
            windowButton.SetActive(true);
            goDeskButton.SetActive(true);
            goClosetButton.SetActive(false);
            rightView.SetActive(false);
            goBoardButton.SetActive(false);
            goChesteButton.SetActive(true);
            goStandButton.SetActive(true);
            goMedalButton.SetActive(true);
            catButton.SetActive(true);
        }
    }

    public void downMain()
    {
        if (frontView.activeSelf && screwdriverTaken)
        {
            frontView.SetActive(false);
            catDialogue.SetActive(false);
            downScrewlessDriverView.SetActive(true);
            downButton.SetActive(false);
            rightButton.SetActive(false);
            leftButton.SetActive(false);
            upButton.SetActive(true);
            rugButton.SetActive(true);
            windowButton.SetActive(false);
            goDeskButton.SetActive(false);
        }

        if (frontView.activeSelf)
        {
            frontView.SetActive(false);
            catDialogue.SetActive(false);
            downView.SetActive(true);
            downButton.SetActive(false);
            rightButton.SetActive(false);
            leftButton.SetActive(false);
            upButton.SetActive(true);
            rugButton.SetActive(true);
            windowButton.SetActive(false);
            goDeskButton.SetActive(false);
            goChesteButton.SetActive(false);
            goStandButton.SetActive(false);
            goMedalButton.SetActive(false);
        }

        if (frontWindowView.activeSelf || frontOpenWindowView.activeSelf)
        {
            frontWindowView.SetActive(false);
            frontOpenWindowView.SetActive(false);
            frontView.SetActive(true);
            rightButton.SetActive(true);
            leftButton.SetActive(true);
            windowButton.SetActive(true);
            bigWindowButton.SetActive(false);
            goBackyardButton.SetActive(false);
            goDeskButton.SetActive(true);
            goChesteButton.SetActive(true);
            goStandButton.SetActive(true);
            goMedalButton.SetActive(true); 
            windowButton.SetActive(true);
            catButton.SetActive(true);
        }

        if (backyardView.activeSelf)
        {
            backyardView.SetActive(false);
            goBackyardButton.SetActive(true);
            frontOpenWindowView.SetActive(true);
            crowPopDialogue.SetActive(false);
            submitCrowCoin.SetActive(false);
        }

        if (frontDeskView.activeSelf)
        {
            frontDeskView.SetActive(false);
            frontView.SetActive(true);
            rightButton.SetActive(true);
            leftButton.SetActive(true);
            computerButton.SetActive(false);
            goChesteButton.SetActive(true);
            goDeskButton.SetActive(true);
            goStandButton.SetActive(true);
            goMedalButton.SetActive(true); 
            windowButton.SetActive(true);
            goDrawersButton.SetActive(false);
            catButton.SetActive(true);
        }

        if (frontComputerView.activeSelf || frontComputerUnlockView.activeSelf)
        {
            frontDeskView.SetActive(true);
            frontComputerUnlockView.SetActive(false);
            frontComputerView.SetActive(false);
            submitCompCodeButton.SetActive(false);
            showComputerCodeInput(false);
            tempComputerCodeFinishButton.SetActive(false);
            computerButton.SetActive(true);
            goDrawersButton.SetActive(true);
            goGameButton.SetActive(false);
            goLetterButton.SetActive(false);
            compDialogue.SetActive(false);
        }

        if (rightPaintingView.activeSelf || rightPaintingUnlockedView.activeSelf)
        {
            rightView.SetActive(true);
            leftButton.SetActive(true);
            downButton.SetActive(false);
            rightPaintingUnlockedView.SetActive(false);
            rightPaintingView.SetActive(false);
            tempPaintingButton.SetActive(false);
            submitPaintingButton.SetActive(false);
            bookshelfButton.SetActive(true);
            paintingButton.SetActive(true);
            goBoardButton.SetActive(true);
            tempGetKeyFromCrow.SetActive(false);
        }

        if (rightBookshelfView.activeSelf)
        {
            rightView.SetActive(true);
            leftButton.SetActive(true);
            downButton.SetActive(false);
            rightBookshelfView.SetActive(false);
            submitPaintingButton.SetActive(false);
            bookshelfButton.SetActive(true);
            paintingButton.SetActive(true);
            goBoardButton.SetActive(true);
            goBookPopButton.SetActive(false);
            bookDialogue.SetActive(false);
        }

        if (frontChestView.activeSelf)
        {
            frontChestView.SetActive(false);
            frontView.SetActive(true);
            rightButton.SetActive(true);
            leftButton.SetActive(true);
            goDeskButton.SetActive(true);
            goChesteButton.SetActive(true);
            goStandButton.SetActive(true);
            goMedalButton.SetActive(true); 
            windowButton.SetActive(true);
            catButton.SetActive(true);
            ChestKeyButton.SetActive(false);

        }
        
        if (frontStandView.activeSelf)
        {
            frontStandView.SetActive(false);
            frontView.SetActive(true);
            rightButton.SetActive(true);
            leftButton.SetActive(true);
            goDeskButton.SetActive(true);
            goChesteButton.SetActive(true);
            goStandButton.SetActive(true);
            goMedalButton.SetActive(true); 
            windowButton.SetActive(true);
            goStandPopButton.SetActive(false);
            catButton.SetActive(true);
            standDialogue.SetActive(false);
        }

        if (frontMedalView.activeSelf)
        {
            frontMedalView.SetActive(false);
            frontView.SetActive(true);
            rightButton.SetActive(true);
            leftButton.SetActive(true);
            goDeskButton.SetActive(true);
            goChesteButton.SetActive(true);
            goStandButton.SetActive(true);
            goMedalButton.SetActive(true); 
            windowButton.SetActive(true);
            catButton.SetActive(true);
            medalDialogue.SetActive(false);
        }
        if (rightBoardView.activeSelf)
        {
            rightView.SetActive(true);
            leftButton.SetActive(true);
            downButton.SetActive(false);
            submitPaintingButton.SetActive(false);
            bookshelfButton.SetActive(true);
            paintingButton.SetActive(true);
            goBoardButton.SetActive(true);
            rightBoardView.SetActive(false);
            tempBoardButton.SetActive(false);
            boardDialogue.SetActive(false);
        }

        if (frontDrawerView.activeSelf)
        {
            frontDeskView.SetActive(true);
            frontDrawerView.SetActive(false);
            goDrawersButton.SetActive(true);
            computerButton.SetActive(true);
            collectDrawercoins.SetActive(false);
        }

        if (leftClosetView.activeSelf || leftClosetUnlockedView.activeSelf)
        {
            leftView.SetActive(true);
            rightButton.SetActive(true);
            downButton.SetActive(false);
            goClosetButton.SetActive(true);
            leftClosetView.SetActive(false);
            submitClosetKey.SetActive(false);
            leftClosetUnlockedView.SetActive(false);
            leftClosetView.SetActive(false);
            leftClosetUnlockednoToyView.SetActive(false);
            getToy.SetActive(false);
        }
    }

    public void upMain()
    {
        if (downView.activeSelf || downScrewDriverView.activeSelf || downScrewlessDriverView.activeSelf)
        {
            downView.SetActive(false);
            frontView.SetActive(true);
            upButton.SetActive(false);
            rightButton.SetActive(true);
            leftButton.SetActive(true);
            downButton.SetActive(true);
            downScrewDriverView.SetActive(false);
            rugButton.SetActive(false);
            windowButton.SetActive(true);
            goDeskButton.SetActive(true);
            screwdriverButton.SetActive(false);
            downScrewlessDriverView.SetActive(false);
            goChesteButton.SetActive(true);
            goStandButton.SetActive(true);
            goMedalButton.SetActive(true);
            catButton.SetActive(true);
        }
    }

    public void rugClick()
    {
        rugButton.SetActive(false);
        downScrewDriverView.SetActive(true);
        downView.SetActive(false);
        screwdriverButton.SetActive(true);
    }

    public void windowClick()
    {
        frontView.SetActive(false);
        catDialogue.SetActive(false);
        frontWindowView.SetActive(true);
        rightButton.SetActive(false);
        leftButton.SetActive(false);
        windowButton.SetActive(false);
        bigWindowButton.SetActive(true);
        goDeskButton.SetActive(false);
        goChesteButton.SetActive(false);
        goStandButton.SetActive(false);
        goMedalButton.SetActive(false); 
        windowButton.SetActive(false);
        
        if (screwdriverUsed)
        {
            frontOpenWindowView.SetActive(true);
            bigWindowButton.SetActive(false);
            goBackyardButton.SetActive(true);
        }
    }

    public void screwdriverClick()
    {
        downScrewlessDriverView.SetActive(true);
        downScrewDriverView.SetActive(false);
        inventory.AddItem("screwdriver");
        foreach (string item in inventory.inventoryItems)
        {
            Debug.Log(item);
        }
        screwdriverTaken = true;
    }

    public void useScrewDriver()
    {
        if (inventory.inventoryItems.Contains("screwdriver"))
        {
            inventory.RemoveItem("screwdriver");
            frontOpenWindowView.SetActive(true);
            screwdriverUsed = true;
            bigWindowButton.SetActive(false);
            goBackyardButton.SetActive(true);
        }
    }

    public void goOutside()
    {
        if (goBackyardButton.activeSelf)
        {
            backyardView.SetActive(true);
            bigWindowButton.SetActive(false);
            frontOpenWindowView.SetActive(false);
            frontWindowView.SetActive(false);
            crowPopDialogue.SetActive(true);
            submitCrowCoin.SetActive(true);
            goBackyardButton.SetActive(false);
            if (submittedGoldCoins)
            {
                crowPopDialogue.SetActive(false);
            }
        }
        
    }

    public void clickDesk()
    {
        frontDeskView.SetActive(true);
        frontView.SetActive(false);
        catDialogue.SetActive(false);
        rightButton.SetActive(false);
        leftButton.SetActive(false);
        windowButton.SetActive(false);
        computerButton.SetActive(true);
        goChesteButton.SetActive(false);
        goMedalButton.SetActive(false);
        goStandButton.SetActive(false);
        goDeskButton.SetActive(false);
        goDrawersButton.SetActive(true);
    }

    public void clickComputer()
    {
        frontDeskView.SetActive(false);
        frontComputerView.SetActive(true);
        // temp unlock button hidden now that the password box works
        // tempComputerCodeFinishButton.SetActive(true);
        submitCompCodeButton.SetActive(true);
        showComputerCodeInput(true);
        computerButton.SetActive(false);
        goDrawersButton.SetActive(false);

        if (computerPasswrod)
        {
            frontComputerUnlockView.SetActive(true);
            tempComputerCodeFinishButton.SetActive(false);
            submitCompCodeButton.SetActive(false);
            showComputerCodeInput(false);
        }
    }

    public void clickSubmitComputerCode()
    {
        if (!computerPasswrod && computerCodeInput != null)
        {
            if (computerCodeInput.text.Trim() == computerCode)
            {
                computerPasswrod = true;
            }
            else
            {
                Debug.Log("Wrong computer password");
                computerCodeInput.text = "";
                passwordErrorText.gameObject.SetActive(true);
            }
        }

        if (computerPasswrod)
        {
            frontComputerUnlockView.SetActive(true);
            tempComputerCodeFinishButton.SetActive(false);
            submitCompCodeButton.SetActive(false);
            showComputerCodeInput(false);
            frontComputerView.SetActive(false);
            goGameButton.SetActive(true);
            goLetterButton.SetActive(true);
            compDialogue.SetActive(true);

        }
    }

    void createComputerCodeInput()
    {
        GameObject inputObject = TMP_DefaultControls.CreateInputField(new TMP_DefaultControls.Resources());
        inputObject.name = "ComputerCodeInput";

        Transform submitButton = submitCompCodeButton.transform;
        inputObject.transform.SetParent(submitButton.parent, false);
        inputObject.transform.SetSiblingIndex(submitButton.GetSiblingIndex());

        // sit directly left of the Login button, on the same line
        RectTransform buttonRect = submitButton.GetComponent<RectTransform>();
        RectTransform rect = inputObject.GetComponent<RectTransform>();
        float width = 500f;
        float gap = 50f;
        float buttonWidth = buttonRect.rect.width * buttonRect.localScale.x;
        float buttonHeight = buttonRect.rect.height * buttonRect.localScale.y;
        rect.sizeDelta = new Vector2(width, buttonHeight);
        rect.anchoredPosition = new Vector2(
            buttonRect.anchoredPosition.x - buttonWidth / 2f - gap - width / 2f,
            buttonRect.anchoredPosition.y);

        computerCodeInput = inputObject.GetComponent<TMP_InputField>();
        computerCodeInput.characterLimit = 10;
        computerCodeInput.textComponent.fontSize = 80;
        computerCodeInput.textComponent.alignment = TextAlignmentOptions.Center;

        TMP_Text placeholder = (TMP_Text)computerCodeInput.placeholder;
        placeholder.text = "Password";
        placeholder.fontSize = 80;
        placeholder.alignment = TextAlignmentOptions.Center;

        inputObject.SetActive(false);
    }

    void showComputerCodeInput(bool show)
    {
        if (computerCodeInput == null)
            return;

        computerCodeInput.text = "";
        passwordErrorText.gameObject.SetActive(false);
        computerCodeInput.gameObject.SetActive(show);
    }

    // small red message just above the password box; hidden again once the player types
    void createPasswordErrorText()
    {
        GameObject errorObject = new GameObject("PasswordError", typeof(RectTransform));
        errorObject.transform.SetParent(computerCodeInput.transform, false);

        RectTransform rect = errorObject.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0f, 1f);
        rect.anchorMax = new Vector2(1f, 1f);
        rect.pivot = new Vector2(0.5f, 0f);
        rect.anchoredPosition = new Vector2(0f, 10f);
        rect.sizeDelta = new Vector2(0f, wrongPasswordFontSize * 1.5f);

        passwordErrorText = errorObject.AddComponent<TextMeshProUGUI>();
        passwordErrorText.text = wrongPasswordMessage;
        passwordErrorText.color = wrongPasswordColor;
        passwordErrorText.fontSize = wrongPasswordFontSize;
        passwordErrorText.alignment = TextAlignmentOptions.Left;
        passwordErrorText.raycastTarget = false;

        computerCodeInput.onValueChanged.AddListener(text =>
        {
            if (text.Length > 0)
                passwordErrorText.gameObject.SetActive(false);
        });

        errorObject.SetActive(false);
    }

    public void clickPainting()
    {
        tempPaintingButton.SetActive(true);
        rightPaintingView.SetActive(true);
        windowButton.SetActive(false);
        rightButton.SetActive(false);
        leftButton.SetActive(false);
        downButton.SetActive(true);
        submitPaintingButton.SetActive(true);
        bookshelfButton.SetActive(false);
        paintingButton.SetActive(false);
        goBoardButton.SetActive(false);

        if (paintingPuzzle)
        {
            rightPaintingUnlockedView.SetActive(true);
            submitPaintingButton.SetActive(false);
            rightPaintingView.SetActive(false); 
            tempPaintingButton.SetActive(false); 
        }
    }

    public void clickBookshelf()
    {
        rightBookshelfView.SetActive(true);
        windowButton.SetActive(false);
        rightButton.SetActive(false);
        leftButton.SetActive(false);
        downButton.SetActive(true);
        paintingButton.SetActive(false);
        bookshelfButton.SetActive(false);
        goBoardButton.SetActive(false);
        goBookPopButton.SetActive(true);
        bookDialogue.SetActive(true);
    }

    public void clickMedals()
    {
        frontMedalView.SetActive(true);
        frontView.SetActive(false);
        catDialogue.SetActive(false);
        rightButton.SetActive(false);
        leftButton.SetActive(false);
        goDeskButton.SetActive(false);
        goChesteButton.SetActive(false);
        goStandButton.SetActive(false);
        windowButton.SetActive(false);
        goMedalButton.SetActive(false);
        medalDialogue.SetActive(true);
    }

    public void clickChest()
    {
        frontChestView.SetActive(true);
        frontView.SetActive(false);
        catDialogue.SetActive(false);
        rightButton.SetActive(false);
        leftButton.SetActive(false);
        goDeskButton.SetActive(false);
        goStandButton.SetActive(false);
        goMedalButton.SetActive(false);      
        windowButton.SetActive(false); 
        goChesteButton.SetActive(false);       
        ChestKeyButton.SetActive(true);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     
    }

    public void clickStand()
    {
        frontStandView.SetActive(true);
        frontView.SetActive(false);
        catDialogue.SetActive(false);
        rightButton.SetActive(false);
        leftButton.SetActive(false);
        goDeskButton.SetActive(false);
        goChesteButton.SetActive(false);
        goMedalButton.SetActive(false);  
        windowButton.SetActive(false);     
        goStandButton.SetActive(false); 
        goStandPopButton.SetActive(true);  
        standDialogue.SetActive(true);
    }

    public void clickBoard()
    {
        rightBoardView.SetActive(true);
        windowButton.SetActive(false);
        rightButton.SetActive(false);
        leftButton.SetActive(false);
        downButton.SetActive(true);
        paintingButton.SetActive(false);
        bookshelfButton.SetActive(false);
        goBoardButton.SetActive(false);
        boardDialogue.SetActive(true);
    }

    public void clickCloset()
    {
        leftButton.SetActive(false);
        rightButton.SetActive(false);
        leftClosetView.SetActive(true);
        leftView.SetActive(false);
        downButton.SetActive(true);
        submitClosetKey.SetActive(true);
        goClosetButton.SetActive(false);
        if (closetUnlocked)
        {
            leftClosetUnlockedView.SetActive(true);
        }

        if (toyCollected)
        {
            leftClosetUnlockednoToyView.SetActive(true);
        }
    }

    public void unlockCloset()
    {
        if (inventory.HasItem("ClosetKey"))
        {
            leftClosetUnlockedView.SetActive(true);
            submitClosetKey.SetActive(false);
            closetUnlocked = true; 
            getToy.SetActive(true); 
        }
    }

    public void collectToy()
    {
        leftClosetUnlockednoToyView.SetActive(true);
        inventory.AddItem("Toy");
        getToy.SetActive(false);
        toyCollected = true;    
    }

    public void clickCat()
    {
        catDialogue.SetActive(true);
        if (inventory.HasItem("Toy"))
        {
            frontCatMoveView.SetActive(true);
            catButton.SetActive(false);
            goPapersButton.SetActive(true);
            catDialogue.SetActive(false);
        }
    }

    public void clickPapers()
    {
        popPapers.SetActive(true);
    }

    public void clickDrawers()
    {
        frontDrawerView.SetActive(true);
        frontDeskView.SetActive(false);
        goDrawersButton.SetActive(false);
        computerButton.SetActive(false);
        collectDrawercoins.SetActive(true);
        
    }

    public void clickPopGame()
    {
        popComputerGame.SetActive(true);
        compDialogue.SetActive(false);
        downButton.SetActive(false);
        goGameButton.SetActive(false);
        goLetterButton.SetActive(false);
    }

    public void clickPopBook()
    {
        popBookSort.SetActive(true);
        downButton.SetActive(false);
        goBookPopButton.SetActive(false);
    }

    public void clickPopStand()
    {
        popStandSort.SetActive(true);
        downButton.SetActive(false);
        goStandPopButton.SetActive(false);
    }

    public void clickPopLetter()
    {
        popComputerLetter.SetActive(true);
        downButton.SetActive(false);
        goGameButton.SetActive(false);
        goLetterButton.SetActive(false);
    }

    public void xPopGame()
    {
        popComputerGame.SetActive(false);
        downButton.SetActive(true);
        goGameButton.SetActive(true);
        goLetterButton.SetActive(true);
    }

    public void xPopBook()
    {
        popBookSort.SetActive(false);
        downButton.SetActive(true);
        goBookPopButton.SetActive(true);
    }

    public void xPopStand()
    {
        popStandSort.SetActive(false);
        downButton.SetActive(true);
        goStandPopButton.SetActive(true);
    }

    public void xPopPapers()
    {
        popPapers.SetActive(false);
        goPapersButton.SetActive(true);
    }

    public void xPopLetter()
    {
        popComputerLetter.SetActive(false);
        downButton.SetActive(true);
        goGameButton.SetActive(true);
        goLetterButton.SetActive(true);
    }

    public void clickTempGameSolve()
    {
        TempCompSolved.SetActive(true);
        TempCompSolvedButton.SetActive(false);
    }

    public void clickTempBookSolve()
    {
        TempBookSolved.SetActive(true);
        TempBookSolvedButton.SetActive(false);
    }

    public void clickTempStandSolve()
    {
        TempStandSolved.SetActive(true);
        TempStandSolvedButton.SetActive(false);
    }

    public void submitPainting()
    {
        if (paintingPuzzle)
        {
            rightPaintingUnlockedView.SetActive(true);
            submitPaintingButton.SetActive(false);
            rightPaintingView.SetActive(false); 
            tempPaintingButton.SetActive(false);
        }
    } 

    public void tempFinishComputerPassword()
    {
        computerPasswrod = true;
        clickSubmitComputerCode();
    }

    public void tempFinishPainting()
    {
        paintingPuzzle = true;
    }

    public void tempFinishBoard()
    {
        boardPuzzle = true;
    }

    public void addBookCoin()
    {
        inventory.AddItem("BookCoin");
        foreach (string item in inventory.inventoryItems)
        {
            Debug.Log(item);
        }
    }

    public void addDoorKey()
    {
        inventory.AddItem("DoorKey");
        foreach (string item in inventory.inventoryItems)
        {
            Debug.Log(item);
        }
    }

    public void addStandCoin()
    {
        inventory.AddItem("StandCoin");
        foreach (string item in inventory.inventoryItems)
        {
            Debug.Log(item);
        }
    }

    public void addDrawerCoin()
    {
        if (boardPuzzle)
        {
            inventory.AddItem("DrawerCoin");
        }
        foreach (string item in inventory.inventoryItems)
        {
            Debug.Log(item);
        }
    }

    public void submitCrowCoins()
    {
        if(inventory.HasItem("DrawerCoin") && inventory.HasItem("StandCoin") && inventory.HasItem("BookCoin"))
        {
            inventory.RemoveItem("DrawerCoin");
            inventory.RemoveItem("StandCoin");
            inventory.RemoveItem("BookCoin");

            tempGetKeyFromCrow.SetActive(true);
            submitCrowCoin.SetActive(false);
            submittedGoldCoins = true; 
            foreach (string item in inventory.inventoryItems)
            {
                Debug.Log(item);
            }       
            Debug.Log("coins have been collected!"); 
        } else
        {
            Debug.Log("not enough coins");
        }
    }

    public void getKeyFromCrow()
    {
        inventory.AddItem("ClosetKey");
        tempGetKeyFromCrow.SetActive(false);
        foreach (string item in inventory.inventoryItems)
            {
                Debug.Log(item);
            }       
    }
}
