//using System;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Purchasing;
//
//public class IAPScript : MonoBehaviour, IStoreListener
//{
//    private static IStoreController m_StoreController;          // The Unity Purchasing system.
//    private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.
//
//	public static string kProductIDConsumable =    "crossy_chicken_support";
//	public static string kProductIDNonConsumable = "crossy_chicken_remove_ads";
//	private static string kProductNameGooglePlaySubscription =  "com.codingshophouse.crossychicken"; 
//
//
//
//    void Start()
//    {
//        // If we haven't set up the Unity Purchasing reference
//        if (m_StoreController == null)
//        {
//            // Begin to configure our connection to Purchasing
//            InitializePurchasing();
//        }
//    }
//    
//    public void InitializePurchasing() 
//    {
//        // If we have already connected to Purchasing ...
//        if (IsInitialized())
//        {
//            // ... we are done here.
//            return;
//        }
//
//        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
//		builder.AddProduct(kProductIDNonConsumable, ProductType.NonConsumable);
//		builder.AddProduct(kProductIDConsumable, ProductType.Consumable);
//        UnityPurchasing.Initialize(this, builder);
//    }
//    
//    
//    private bool IsInitialized()
//    {
//        // Only say we are initialized if both the Purchasing references are set.
//        return m_StoreController != null && m_StoreExtensionProvider != null;
//    }
//    
//    
//    public void BuyConsumable()
//    {
//		print("BuyConsumable");
//        BuyProductID(kProductIDConsumable);
//    }
//    
//    
//    public void BuyNonConsumable()
//    {
//		print("BuyNonConsumable");
//        BuyProductID(kProductIDNonConsumable);
//    }
//    
//
//    void BuyProductID(string productId)
//    {
//		print("BuyProductID");
//
//        if (IsInitialized())
//        {
//            Product product = m_StoreController.products.WithID(productId);
//
//            if (product != null && product.availableToPurchase)
//            {
//                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
//                m_StoreController.InitiatePurchase(product);
//            }
//            else
//            {
//                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
//            }
//        }
//        else
//        {
//            Debug.Log("BuyProductID FAIL. Not initialized.");
//        }
//    }
//
//
//
//    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
//    {
//        Debug.Log("OnInitialized: PASS");
//
//        m_StoreController = controller;
//        m_StoreExtensionProvider = extensions;
//    }
//    
//    
//    public void OnInitializeFailed(InitializationFailureReason error)
//    {
//        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
//    }
//    
//    
//    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) 
//    {
//        if (String.Equals(args.purchasedProduct.definition.id, kProductIDConsumable, StringComparison.Ordinal))
//        {
//            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));// The consumable item has been successfully purchased, add 100 coins to the player's in-game score.
//            //ScoreManager.score += 100;
//        }
//        else if (String.Equals(args.purchasedProduct.definition.id, kProductIDNonConsumable, StringComparison.Ordinal))
//        {
//            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));// TODO: The non-consumable item has been successfully purchased, grant this item to the player.
//        }
//        else 
//        {
//            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
//        }
//
//        return PurchaseProcessingResult.Complete;
//    }
//    
//    
//    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
//    {
//        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
//    }
//}