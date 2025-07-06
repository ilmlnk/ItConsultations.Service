const admin = require('firebase-admin');

const serviceAccount = require('./firebase-service-account.json');

admin.initializeApp({
  credential: admin.credential.cert(serviceAccount),
  projectId: 'itconsultations-436110'
});

async function createUserAndGetToken() {
  try {
    const userRecord = await admin.auth().createUser({
      email: 'test@example.com',
      password: 'password123',
      displayName: 'Test User'
    });

    console.log('User created:', userRecord.uid);

    const customToken = await admin.auth().createCustomToken(userRecord.uid);
    console.log('Custom Token:', customToken);

    const idToken = await admin.auth().createSessionCookie(customToken, {
      expiresIn: 60 * 60 * 24 * 5 * 1000 // 5 days
    });

    console.log('Session Cookie (ID Token):', idToken);

  } catch (error) {
    console.error('Error:', error);
  }
}

createUserAndGetToken(); 