using IronPdf;

class PdfGenerator
{
    public static void GenerateFeaturesPdf(string filePath)
    {
        var Renderer = new ChromePdfRenderer();

        string htmlContent = @"
            <h1>Fonctionnalités du gestionnaire de mots de passe</h1>
            
            <h2>Connexion</h2>
            <p>La fonctionnalité de connexion permet aux utilisateurs de saisir leurs identifiants et d'accéder à leur compte de manière sécurisée.</p>

            <h2>Inscription</h2>
            <p>Le processus d'inscription permet aux nouveaux utilisateurs de créer un compte en fournissant les informations nécessaires telles que le nom d'utilisateur, l'email et le mot de passe.</p>

            <h2>Se souvenir de moi</h2>
            <p>Cette option garde l'utilisateur connecté sur l'appareil en sauvegardant les jetons d'authentification de façon sécurisée, évitant ainsi la saisie répétée des identifiants.</p>

            <h2>Importation CSV</h2>
            <p>La fonctionnalité d'importation permet aux utilisateurs de charger un fichier CSV pour ajouter plusieurs mots au dictionnaire, facilitant ainsi la génération ou la vérification des mots de passe.</p>

            <h2>Ajouter un compte</h2>
            <p>Cette fonctionnalité permet aux utilisateurs d'ajouter de nouveaux profils (comptes) dans le gestionnaire de mots de passe, en enregistrant des informations comme le nom d'utilisateur, le mot de passe, l'URL, etc.</p>

            <h2>Barre de recherche</h2>
            <p>La barre de recherche aide les utilisateurs à trouver rapidement des comptes ou mots de passe enregistrés en tapant des mots-clés.</p>

            <h2>Générer un mot de passe sécurisé</h2>
            <p>Génère un mot de passe aléatoire et sécurisé avec une longueur spécifiée, incluant des lettres, chiffres et caractères spéciaux pour renforcer la sécurité.</p>
        ";

        var pdf = Renderer.RenderHtmlAsPdf(htmlContent);

        pdf.SaveAs(filePath);
    }
}
